using System;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// Static utility for obtaining various game-related information and metadata.
    /// </summary>
    public static class SInfos
    {
        /// <summary>
        /// Current game name.
        /// </summary>
        public static string GameName => "Stellar Duelist";

        /// <summary>
        /// Current game version.
        /// </summary>
        public static Version GameVersion => new(1, 0, 4, 6);

        /// <summary>
        /// Get a formatted title that combines <see cref="GameName"/> and <see cref="GameVersion"/>.
        /// </summary>
        /// <returns>The formatted title.</returns>
        public static string GetTitle()
        {
            return $"{GameName} - {GameVersion}";
        }
    }
}
