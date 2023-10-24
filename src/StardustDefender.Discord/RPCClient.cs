using DiscordRPC;

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
            this._client = new(Environment.GetEnvironmentVariable("DISCORD_RPC_CLIENT_ID"));
            this._presence = new();

            this.initializeUnixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _ = this._client.Initialize();
            _ = Task.Run(UpdateAsync);
        }
        public void Stop()
        {
            this._client.Dispose();
        }

        private async Task UpdateAsync()
        {
            try
            {
                _ = this._presence.WithTimestamps(new() { StartUnixMilliseconds = this.initializeUnixTimestamp });
                _ = this._presence.WithAssets(new()
                {
                    LargeImageKey = "large_1",
                    LargeImageText = SInfos.GetTitle(),
                });

                while (!this._client.IsDisposed)
                {
                    await GetInfosAsync();

                    _ = this._presence.WithDetails(this.details);
                    _ = this._presence.WithState(this.state);
                    this._client.SetPresence(this._presence);
                    
                    await Task.Delay(TimeSpan.FromSeconds(2f));
                }
            }
            catch (Exception)
            {
                await Task.CompletedTask;
            }
            finally
            {
                if (!this._client.IsDisposed)
                {
                    this._client.Dispose();
                }
            }
        }
        private async Task GetInfosAsync()
        {
            switch (SGameController.State)
            {
                case SGameState.Introduction:
                    this.details = "Starting a new adventure!";
                    this.state = "On Introduction.";
                    break;

                case SGameState.Running:
                    if (SLevelController.BossAppeared)
                    {
                        this.details = $"Battling a Boss.";
                        this.state = GetStatusString();
                    }
                    else
                    {
                        this.details = $"Battling {SDifficultyController.TotalEnemyCount - SLevelController.EnemiesKilled} enemies.";
                        this.state = GetStatusString();
                    }

                    break;

                case SGameState.Paused:
                    this.details = "Paused.";
                    this.state = string.Empty;
                    break;

                case SGameState.Victory:
                    this.details = $"Victory.";
                    this.state = $"Finished level {SLevelController.Level + 1}.";
                    break;

                case SGameState.GameOver:
                    this.details = "Game Over!";
                    this.state = string.Empty;
                    break;

                default:
                    this.details = "Idle.";
                    this.state = string.Empty;
                    break;
            }

            await Task.CompletedTask;

            string GetStatusString()
            {
                ReadOnlySpan<char> s_lvl = (SLevelController.Level + 1).ToString();
                ReadOnlySpan<char> s_hp = SLevelController.Player.HealthValue.ToString();
                ReadOnlySpan<char> s_atk = SLevelController.Player.AttackValue.ToString();

                return $"LVL: {s_lvl} | HP: {s_hp} | ATK: {s_atk}";
            }
        }
    }
}