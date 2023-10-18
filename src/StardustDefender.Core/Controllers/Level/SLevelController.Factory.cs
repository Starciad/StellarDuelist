using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

using System;

namespace StardustDefender.Core.Controllers
{
    public static partial class SLevelController
    {
        private static void CreatePlayer()
        {
            Type playerType = Array.Find(SEntityManager.EntityHeaders, x => x.Classification == SEntityClassification.Player).EntityType;

            player = (SPlayerEntity)SEntityManager.Create(playerType, playerPosition);
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
