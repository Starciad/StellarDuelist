using Microsoft.Xna.Framework;

using StardustDefender.Controllers;

namespace StardustDefender.Entities.Aliens
{
    internal abstract class SAlien : SEntity
    {
        protected void CollideWithPlayer()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, WorldPosition) < 32)
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }
        }
    }
}
