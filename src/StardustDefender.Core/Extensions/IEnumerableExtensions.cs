using StardustDefender.Core.Components;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Core.Extensions
{
    /// <summary>
    /// Provides extensions for IEnumerable collections.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Selects a random element from the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="values">The collection from which to select a random element.</param>
        /// <returns>The randomly selected element.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the input collection is empty.</exception>
        public static T SelectRandom<T>(this IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            int count = values.Count();
            return count == 0 ? default : count == 1 ? values.ElementAtOrDefault(0) : values.ElementAtOrDefault(SRandom.Range(0, count));
        }
    }
}
