using Microsoft.Xna.Framework;

using StardustDefender.Controllers;

namespace StardustDefender.Core.Entities.Templates
{
    public abstract class SBossEntity : SEntity
    {
        protected void CollideWithPlayer()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, this.WorldPosition) < 64)
            {
                SLevelController.Player.Damage(1);
            }
        }
    }
}
