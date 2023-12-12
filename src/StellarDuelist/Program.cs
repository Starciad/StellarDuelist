using System;

using StellarDuelist.Core;
using StellarDuelist.Core.System;
using StellarDuelist.Core.IO;

#if PC
using StellarDuelist.Discord;
#endif

#if WINDOWS_DX
using System.Text;
using System.Windows.Forms;

using StellarDuelist.Core.Engine;
#endif

namespace StellarDuelist.Game
{
    internal static class Program
    {
#if PC
        private static readonly RPCClient _rpcClient = new();
#endif

        [STAThread]
        private static void Main()
        {
            try
            {
                SEnvironment.Initialize();
                SDirectory.Initialize();

#if PC
                _rpcClient.Start();
#endif
            }
            catch (Exception e)
            {
                HandleException(e);
                return;
            }

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
                HandleException(e);
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

        private static void HandleException(Exception value)
        {
#if WINDOWS_DX
            string logFilename = SFile.WriteException(value);
            StringBuilder logString = new();
            logString.AppendLine("An unexpected error caused StellarDuelist to crash!");
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{SInfos.GetTitle()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#else
                _ = SFile.WriteException(value);
#endif
        }
    }
}