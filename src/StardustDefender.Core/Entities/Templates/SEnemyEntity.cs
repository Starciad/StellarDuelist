using Microsoft.Xna.Framework;

using StardustDefender.Core.Controllers;

namespace StardustDefender.Core.Entities.Templates
{
    public abstract class SEnemyEntity : SEntity
    {
        protected void CollideWithPlayer()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, this.WorldPosition) < 32)
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }
        }
    }
}
