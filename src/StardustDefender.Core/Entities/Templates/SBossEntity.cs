using Microsoft.Xna.Framework;

using StardustDefender.Core.Controllers;

namespace StardustDefender.Core.Entities.Templates
{
    /// <summary>
    /// Base class template for creating bosses entities.
    /// </summary>
    /// <remarks>
    /// With this template, a variety of functions, properties, and attributes are provided to automate certain processes and have references for internal work/configurations.
    /// </remarks>
    public abstract class SBossEntity : SEntity
    {
        protected void CollideWithPlayer()
        {
            if (this.CollisionBox.Intersects(SLevelController.Player.CollisionBox))
            {
                SLevelController.Player.Damage(1);
            }
        }
    }
}
