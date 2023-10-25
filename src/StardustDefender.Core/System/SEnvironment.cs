using System;
using System.IO;

namespace StardustDefender.Core.System
{
    /// <summary>
    /// Utility static manager for environment variables.
    /// </summary>
    public static class SEnvironment
    {
        private static readonly string ENV_FILE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "System", "Environment");

        /// <summary>
        /// Initializes environment variables from a settings file.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown when the "Environment" settings file is not found.</exception>
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
