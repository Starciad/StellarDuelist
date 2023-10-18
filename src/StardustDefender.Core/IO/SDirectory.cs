using System.Collections.Generic;
using System.IO;

namespace StardustDefender.Core.IO
{
    public static class SDirectory
    {
        private static string GAME_DIRECTORY => Directory.GetCurrentDirectory();
        private static string[] DEFAULT_DIRECTORIES => new string[]
        {
            "Logs",
            "Settings",
        };

        private static readonly Dictionary<string, string> directories = new();

        public static void Initialize()
        {
            foreach (string directoryName in DEFAULT_DIRECTORIES)
            {
                string targetDirectoryPath = Path.Combine(GAME_DIRECTORY, directoryName);
                if (!Directory.Exists(targetDirectoryPath))
                {
                    Directory.CreateDirectory(targetDirectoryPath);
                }

                directories.Add(directoryName, targetDirectoryPath);
            }
        }
        public static string GetDirectoryPath(string name)
        {
            return directories[name];
        }
    }
}
