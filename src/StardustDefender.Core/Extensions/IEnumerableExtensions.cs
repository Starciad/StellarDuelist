using StardustDefender.Core.Components;

using System.Collections.Generic;
using System.Linq;
using System;

namespace StardustDefender.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T SelectRandom<T>(this IEnumerable<T> values)
        {
            if (values == null)
                throw new InvalidOperationException("Sequence contains no elements");

            int count = values.Count();
            return count == 0 ? default : count == 1 ? values.ElementAtOrDefault(0) : values.ElementAtOrDefault(SRandom.Range(0, count));
        }
    }
}
