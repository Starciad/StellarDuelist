using StardustDefender.Core;

using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static T SelectRandom<T>(this IEnumerable<T> values)
        {
            int count = values.Count();

            return count == 0 ? default : count == 1 ? values.ElementAtOrDefault(0) : values.ElementAtOrDefault(SRandom.Range(0, count));
        }
    }
}
