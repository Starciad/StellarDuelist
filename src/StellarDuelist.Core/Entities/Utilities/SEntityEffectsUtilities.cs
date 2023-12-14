using Microsoft.Xna.Framework;

using System.Threading.Tasks;

namespace StellarDuelist.Core.Entities.Utilities
{
    public static class SEntityEffectsUtilities
    {
        public static void ApplyTemporaryDamageColor(SEntity entity, int millisecondsDelay)
        {
            _ = Task.Run(async () =>
            {
                entity.Color = Color.Red;
                await Task.Delay(millisecondsDelay);
                entity.Color = Color.White;
            });
        }
    }
}
