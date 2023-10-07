using System;

namespace StardustDefender.Core.Components
{
    public static class SRandom
    {
        private static readonly Random random = new();

        public static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }

        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
