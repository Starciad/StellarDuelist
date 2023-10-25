using System;

namespace StardustDefender.Core.Components
{
    public static class SInfos
    {
        public static string GameName => "Stardust Defender";
        public static Version GameVersion => new(1, 0, 1, 0);

        public static string GetTitle()
        {
            return $"{GameName} - {GameVersion}";
        }
    }
}
