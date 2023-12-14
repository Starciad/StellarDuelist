using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;

using System;

namespace StellarDuelist.Core.Controllers
{
    public static partial class SLevelController
    {
        private static void CreatePlayer()
        {
            Type playerType = Array.Find(SEntityManager.EntityDefinitions, x => x.Classification == SEntityClassification.Player).EntityTargetType;

            player = (SPlayerEntity)SEntityManager.Create(playerType, playerPosition);
            ResetPlayerPosition();
        }
        private static void SpawnEnemy()
        {
            // Spawns enemies if the number of spawned enemies is less than the total.
            if (spawnedEnemies < SDifficultyController.TotalEnemyCount)
            {
                CreateEnemy();
                spawnedEnemies++;
            }
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
