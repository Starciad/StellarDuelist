using StardustDefender.Entities.Player;
using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Managers;

using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace StardustDefender.Entities.Bosses
{
    internal abstract class SBossEntity : SEntity
    {
        protected void CollideWithPlayer()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, WorldPosition) < 64)
            {
                SLevelController.Player.Damage(1);
            }
        }
    }
}
