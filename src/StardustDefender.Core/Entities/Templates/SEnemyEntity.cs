using Microsoft.Xna.Framework;

using StardustDefender.Core.Controllers;

namespace StardustDefender.Core.Entities.Templates
{
    /// <summary>
    /// Base class template for creating enemies entities.
    /// </summary>
    /// <remarks>
    /// With this template, a variety of functions, properties, and attributes are provided to automate certain processes and have references for internal work/configurations.
    /// </remarks>
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
