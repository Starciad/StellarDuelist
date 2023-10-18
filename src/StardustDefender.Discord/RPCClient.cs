using DiscordRPC;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Discord
{
    public sealed class RPCClient
    {
        private DiscordRpcClient _client;
        private RichPresence _presence;

        private ulong initializeUnixTimestamp;
        private string details;
        private string state;

        public void Start()
        {
            _client = new(Environment.GetEnvironmentVariable("DISCORD_RPC_CLIENT_ID"));
            _presence = new();

            initializeUnixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _ = _client.Initialize();
            _ = Task.Run(UpdateAsync);
        }
        public void Stop()
        {
            _client.Dispose();
        }

        private async Task UpdateAsync()
        {
            try
            {
                _presence.WithTimestamps(new() { StartUnixMilliseconds = initializeUnixTimestamp });

                while (!_client.IsDisposed)
                {
                    await GetInfosAsync();

                    _presence.WithDetails(details);
                    _presence.WithState(state);
                    _presence.WithAssets(new()
                    {
                        LargeImageKey = "large_1",
                        LargeImageText = SInfos.GetTitle(),
                    });

                    _client.SetPresence(_presence);

                    await Task.Delay(TimeSpan.FromSeconds(3f));
                }
            }
            catch (Exception)
            {
                await Task.CompletedTask;
            }
            finally
            {
                if (!_client.IsDisposed)
                    _client.Dispose();
            }
        }
        private async Task GetInfosAsync()
        {
            switch (SGameController.State)
            {
                case SGameState.Introduction:
                    details = "Starting a new adventure!";
                    state = "On Introduction.";
                    break;

                case SGameState.Running:
                    if (SLevelController.BossAppeared)
                    {
                        details = $"Battling a Boss.";
                        state = GetStatusString();
                    }
                    else
                    {
                        details = $"Battling {SDifficultyController.TotalEnemyCount - SLevelController.EnemiesKilled} enemies.";
                        state = GetStatusString();
                    }
                    break;

                case SGameState.Paused:
                    details = "Paused.";
                    state = string.Empty;
                    break;

                case SGameState.Victory:
                    details = $"Victory.";
                    state = $"Finished level {SLevelController.Level + 1}.";
                    break;

                case SGameState.GameOver:
                    details = "Game Over!";
                    state = string.Empty;
                    break;

                default:
                    details = string.Empty;
                    state = string.Empty;
                    break;
            }

            await Task.CompletedTask;

            string GetStatusString()
            {
                ReadOnlySpan<char> s_lvl = (SLevelController.Level + 1).ToString();
                ReadOnlySpan<char> s_hp = SLevelController.Player.HealthValue.ToString();
                ReadOnlySpan<char> s_atk = SLevelController.Player.DamageValue.ToString();

                return $"Lvl. {s_lvl} | HP: {s_hp} | ATK: {s_atk}";
            }
        }
    }
}