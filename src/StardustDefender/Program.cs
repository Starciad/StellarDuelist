using System;

using StardustDefender.Core;
using StardustDefender.Core.IO;
using Microsoft.Xna.Framework;

#if PC
using StardustDefender.Discord;
#endif

#if !DEBUG
using StardustDefender.Core.Components;
#endif

#if WINDOWS_DX
using System.Windows.Forms;
#endif

namespace StardustDefender
{
    internal static class Program
    {
#if PC
        private static readonly RPCClient _rpcClient = new();
#endif

        [STAThread]
        private static void Main()
        {
            SDirectory.Initialize();

#if PC
            _rpcClient.Start();
#endif

#if DEBUG
            EXECUTE_DEBUG_VERSION();
#else
            EXECUTE_PUBLISHED_VERSION();
#endif
        }

#if DEBUG
        private static void EXECUTE_DEBUG_VERSION()
        {
            using SGame game = new(typeof(Program).Assembly);
            game.Exiting += OnGameExiting;
            game.Run();
        }

#else
        private static void EXECUTE_PUBLISHED_VERSION()
        {
            using SGame game = new(typeof(Program).Assembly);
            game.Exiting += OnGameExiting;

            try
            {
                game.Run();
            }
            catch (Exception e)
            {
#if WINDOWS_DX
                string logFilename = SFile.WriteException(e);
                MessageBox.Show($"An unexpected error caused StardustDefender to crash!\n\nCheck the log file created at: {logFilename}\n\n\n\n\nException: {e.Message}",
                                $"{SInfos.GetTitle()} - Fatal Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
#else
                _ = SFile.WriteException(e);
#endif
            }
            finally
            {
                game.Dispose();
                game.Exit();
            }
        }
#endif

        private static void OnGameExiting(object sender, EventArgs e)
        {
#if PC
            _rpcClient.Stop();
#endif
        }
    }
}