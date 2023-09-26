using System;

namespace StardustDefender.Core
{
    internal static class SRandom
    {
        private static readonly Random random = new();

        internal static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }

        internal static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        internal static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
