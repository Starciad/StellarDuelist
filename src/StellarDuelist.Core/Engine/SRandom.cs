using System;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// A utility class for generating random values and handling chances.
    /// </summary>
    public static class SRandom
    {
        private static readonly Random random = new();

        /// <summary>
        /// Determines if an event with a given chance occurs within a specified total.
        /// </summary>
        /// <param name="chance">The chance of the event occurring.</param>
        /// <param name="total">The total value for comparison.</param>
        /// <returns>True if the event occurs, false otherwise.</returns>
        public static bool Chance(int chance, int total)
        {
            return Range(0, total) < chance;
        }

        /// <summary>
        /// Generates a random floating-point number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random float between 0.0 (inclusive) and 1.0 (exclusive).</returns>
        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Generates a random integer within a specified range.
        /// </summary>
        /// <param name="min">The inclusive minimum value of the range.</param>
        /// <param name="max">The exclusive maximum value of the range.</param>
        /// <returns>A random integer within the specified range.</returns>
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
