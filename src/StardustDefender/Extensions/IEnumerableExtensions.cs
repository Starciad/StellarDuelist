using StardustDefender.Engine;

using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static T SelectRandom<T>(this IEnumerable<T> values)
        {
            int count = values.Count();

            if (count == 0)
                return default;

            if (count == 1)
                return values.ElementAtOrDefault(0);

            return values.ElementAtOrDefault(SRandom.Range(0, count));
        }
    }
}
