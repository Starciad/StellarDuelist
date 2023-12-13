using Microsoft.Xna.Framework;

namespace StellarDuelist.Core.Entities.Utilities
{
    public static class SEntityUtilities
    {
        public static bool IsColliding(SEntity target1, SEntity target2)
        {
            return target1.CollisionBox.Intersects(target2.CollisionBox);
        }
    }
}
