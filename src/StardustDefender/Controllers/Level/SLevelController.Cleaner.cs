using StardustDefender.Entities.Player;
using StardustDefender.Entities;
using StardustDefender.Items;
using StardustDefender.Managers;
using StardustDefender.World;

using Microsoft.Xna.Framework;

namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
        private static void RemoveObjectsOffScreen()
        {
            RemoveEntitiesOutOfRange(SEntityManager.Entities, minEntityDespawnLimit, maxEntityDespawnLimit);
            RemoveItemsOutOfRange(SItemsManager.Items, minEntityDespawnLimit * SWorld.GridScale, maxEntityDespawnLimit * SWorld.GridScale);
        }
        private static void RemoveEntitiesOutOfRange(SEntity[] entities, Vector2 minLimit, Vector2 maxLimit)
        {
            foreach (SEntity entity in entities)
            {
                if (entity == null)
                {
                    continue;
                }

                if (IsPositionOutOfBounds(entity.LocalPosition, minLimit, maxLimit))
                {
                    if (entity is SPlayerEntity)
                    {
                        continue;
                    }

                    spawnedEnemies--;
                    SEntityManager.Remove(entity);
                }
            }
        }
        private static void RemoveItemsOutOfRange(SItem[] items, Vector2 minLimit, Vector2 maxLimit)
        {
            foreach (SItem item in items)
            {
                if (item == null)
                {
                    continue;
                }

                if (IsPositionOutOfBounds(item.Position, minLimit, maxLimit))
                {
                    SItemsManager.Remove(item);
                }
            }
        }
        private static bool IsPositionOutOfBounds(Vector2 position, Vector2 minLimit, Vector2 maxLimit)
        {
            return position.X < minLimit.X || position.Y < minLimit.Y || position.X > maxLimit.X || position.Y > maxLimit.Y;
        }

        private static void ResetPlayerPosition()
        {
            Player.LocalPosition = playerPosition;
            Player.WorldPosition += SWorld.GetWorldPosition(new(0, 10));
        }

        private static void ResetLevelInfos()
        {
            enemiesKilled = 0;
            spawnedEnemies = 0;
            playerCumulativeDamage = 0;
        }
    }
}
