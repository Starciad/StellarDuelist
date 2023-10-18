using System;
using System.IO;

namespace StardustDefender.Core.System
{
    public static class SEnvironment
    {
        private static readonly string ENV_FILE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "System", "Environment");

        public static void Initialize()
        {
            if (!File.Exists(ENV_FILE_PATH))
            {
                throw new FileNotFoundException($"The \"Environment\" settings file cannot be found in \"{ENV_FILE_PATH}\". Confirm that the file exists on the system and has not been moved or altered in any way.");
            }

            using StreamReader reader = new(ENV_FILE_PATH);
            foreach (string envVar in reader.ReadToEnd().Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] tokens = envVar.Split('=', StringSplitOptions.TrimEntries);

                ReadOnlySpan<char> varName = tokens[0];
                ReadOnlySpan<char> varValue = tokens[1];

                Environment.SetEnvironmentVariable(varName.ToString(), varValue.ToString());
            }
        }
    }
}