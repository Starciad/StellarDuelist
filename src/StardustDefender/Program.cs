using StardustDefender.Core;
using StardustDefender.Core.Components;

using System;
using System.IO;
using System.Windows.Forms;

namespace StardustDefender
{
    internal static class Program
    {
#if !DEBUG
        private static string LOGS_DIRECTORY => Path.Combine(Directory.GetCurrentDirectory(), "Logs");
#endif

        [STAThread]
        private static void Main()
        {
            using SGame game = new(typeof(Program).Assembly);

#if DEBUG
            game.Run();
#else
            ConfigureDirectories();

            try
            {
                game.Run();
            }
            catch (Exception e)
            {
                string logFileName =CreateExceptionLog(e);

#if WindowsDX
                MessageBox.Show($"An unexpected error caused StardustDefender to crash!\n\nCheck the log file created at: {Path.Combine(LOGS_DIRECTORY, logFileName)}\n\n\n\n\nException: {e.Message}",
                                $"{SInfos.GetTitle()} - Fatal Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
#endif
            }
            finally
            {
                game.Dispose();
                game.Exit();
            }
#endif
        }

#if !DEBUG
        private static void ConfigureDirectories()
        {
            if (!Directory.Exists(LOGS_DIRECTORY))
            {
                Directory.CreateDirectory(LOGS_DIRECTORY);
            }
        }

        private static string CreateExceptionLog(Exception exception)
        {
            string dateTimeString = DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
            string fileName = $"StardustDefender_Log_{dateTimeString}.txt";

            File.WriteAllText(Path.Combine(LOGS_DIRECTORY, fileName), exception.ToString());
            return fileName;
        }
#endif
    }
}