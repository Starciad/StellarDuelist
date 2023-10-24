using System;
using System.IO;

namespace StardustDefender.Core.IO
{
    /// <summary>
    /// Provides utility methods for working with files in the game.
    /// </summary>
    public static class SFile
    {
        /// <summary>
        /// Writes an exception to a log file and returns the full path of the log file.
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        /// <returns>The full path of the created log file.</returns>
        public static string WriteException(Exception exception)
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            _ = Directory.CreateDirectory(logDirectory);
            string logFileName = $"StardustDefender_Log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string logFilePath = Path.Combine(logDirectory, logFileName);
            using (StringWriter stringBuilder = new())
            {
                stringBuilder.WriteLine(exception.ToString());
                File.WriteAllText(logFilePath, stringBuilder.ToString());
            }

            return logFilePath;
        }
    }
}
