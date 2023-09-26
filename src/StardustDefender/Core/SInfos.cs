using System;

namespace StardustDefender.Core
{
    internal static class SInfos
    {
        internal static string GameName => "Stardust Defender";
        internal static Version GameVersion => new(1, 0, 0, 0);

        internal static string GetTitle()
        {
            return $"{GameName} - {GameVersion}";
        }
    }
}
