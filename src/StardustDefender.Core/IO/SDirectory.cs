using System.Collections.Generic;
using System.IO;

namespace StardustDefender.Core.IO
{
    /// <summary>
    /// Provides utility methods for working with directories in the game.
    /// </summary>
    public static class SDirectory
    {
        private static string GAME_DIRECTORY => Directory.GetCurrentDirectory();
        private static string[] DEFAULT_DIRECTORIES => new string[]
        {
            "Logs",
            "Settings",
            "Screenshots",
            "Gifs",
            "Scores"
        };

        private static readonly Dictionary<string, string> directories = new();

        /// <summary>
        /// Initializes the default game directories if they don't exist.
        /// </summary>
        public static void Initialize()
        {
            foreach (string directoryName in DEFAULT_DIRECTORIES)
            {
                string targetDirectoryPath = Path.Combine(GAME_DIRECTORY, directoryName);
                if (!Directory.Exists(targetDirectoryPath))
                {
                    _ = Directory.CreateDirectory(targetDirectoryPath);
                }

                directories.Add(directoryName, targetDirectoryPath);
            }
        }

        /// <summary>
        /// Gets the full path of a directory by its name.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        /// <returns>The full path of the directory.</returns>
        public static string GetDirectoryPath(string name)
        {
            return directories[name];
        }
    }
}
