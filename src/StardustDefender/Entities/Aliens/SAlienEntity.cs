using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Core;
using StardustDefender.Managers;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Aliens
{
    internal abstract class SAlienEntity : SEntity
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
