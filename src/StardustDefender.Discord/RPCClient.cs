using DiscordRPC;

using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Core.Discord
{
    public sealed class RPCClient
    {
        private readonly DiscordRpcClient _client = new("1161852580801019965", autoEvents: false);
        private readonly ulong initializeUnixTimestamp;

        private string details;
        private string state;

        public RPCClient()
        {
            initializeUnixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public void Start()
        {
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
                while (!_client.IsDisposed)
                {
                    await GetInfosAsync();

                    _client.SetPresence(new()
                    {
                        Details = details,
                        State = state,
                        Timestamps = new() { StartUnixMilliseconds = initializeUnixTimestamp },
                        Assets = new()
                        {
                            LargeImageKey = "large_1",
                            LargeImageText = SInfos.GetTitle(),
                        }
                    });

                    await Task.Delay(TimeSpan.FromSeconds(2.5f));
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
                    state = "On Introduction.";
                    details = string.Empty;
                    break;

                case SGameState.Running:
                    state = $"Battling {0} enemies. - Level {0} ({0}).";
                    details = $"HP: {0} || ATK: {0}";
                    break;

                case SGameState.Paused:
                    state = "Paused.";
                    details = string.Empty;
                    break;

                case SGameState.Victory:
                    state = $"Victory - Finished level {0}.";
                    details = $"HP: {0} || ATK: {0}";
                    break;

                case SGameState.GameOver:
                    state = "Game Over!";
                    details = "";
                    break;

                default:
                    state = string.Empty;
                    details = string.Empty;
                    break;
            }

            await Task.CompletedTask;
        }
    }
}