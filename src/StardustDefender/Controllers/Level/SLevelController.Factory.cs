using StardustDefender.Core;
using StardustDefender.Entities.Player;
using StardustDefender.Managers;

using System;

namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
        private static void CreatePlayer()
        {
            player = SEntityManager.Create<SPlayerEntity>(playerPosition);
            ResetPlayerPosition();
        }
        private static void CreateEnemy()
        {
            _ = SDifficultyController.CreateRandomEnemy(new(enemyPosition.X + SRandom.Range(-ENEMY_SPAWN_RANGE, ENEMY_SPAWN_RANGE + 1), enemyPosition.Y));
        }
        private static void CreateBoss()
        {
            if (bossTypeSelected == null)
            {
                return;
            }

            _ = SDifficultyController.CreateBossOfType(bossTypeSelected, bossPosition);
            bossDead = false;
        }

        private static bool TrySelectingRandomBoss()
        {
            if (SDifficultyController.TryGetRandomBossType(out Type bossType))
            {
                bossTypeSelected = bossType;
                return true;
            }

            return false;
        }
    }
}
