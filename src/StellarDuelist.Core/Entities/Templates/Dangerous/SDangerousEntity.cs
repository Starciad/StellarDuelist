using StellarDuelist.Core.Controllers;

namespace StellarDuelist.Core.Entities.Templates.Dangerous
{
    /// <summary>
    /// Base class template for creating dangerous entities.
    /// </summary>
    public abstract class SDangerousEntity : SEntity
    {
        protected bool IsCollidingWithThePlayer()
        {
            return this.Collision.IsColliding(SLevelController.Player.Collision);
        }
    }
}