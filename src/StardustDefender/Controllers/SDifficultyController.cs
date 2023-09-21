using Microsoft.Xna.Framework;

using StardustDefender.Engine;
using StardustDefender.Entities;
using StardustDefender.Entities.Aliens;
using StardustDefender.Extensions;
using StardustDefender.Managers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Controllers
{
    internal static class SDifficultyController
    {
        internal static float EnemySpawnDelay => enemySpawnDelay + SRandom.NextFloat();
        internal static int TotalEnemyCount => totalEnemyCount;

        private static float difficultyRate = 0;
        private static float enemySpawnDelay = 0;
        private static int totalEnemyCount = 0;

        private static readonly Dictionary<float, Type> enemiesByDifficulty = new()
        {
            [0f] = typeof(SAlienEntity),
            [2f] = typeof(SAlienShipEntity),
        };

        internal static void BeginRun()
        {
            difficultyRate = 1f;
            enemySpawnDelay = 2.5f;
            totalEnemyCount = SRandom.Range(5, 11);
        }

        internal static void Next()
        {
            // Easy
            if (SLevelController.PlayerCumulativeDamage <= 0)
            {
                difficultyRate++;
                enemySpawnDelay -= SRandom.Range(0, 2) * SRandom.NextFloat();
                totalEnemyCount += SRandom.Range(4, 9);
            }
            else // hard
            {
                difficultyRate--;
                enemySpawnDelay += SRandom.Range(0, 2) * SRandom.NextFloat();
                totalEnemyCount -= SRandom.Range(3, 7);
            }

            if (difficultyRate < 0) difficultyRate = 0;
            if (totalEnemyCount < 5) totalEnemyCount = 5;
            if (enemySpawnDelay < 0) enemySpawnDelay = 0;
        }
        internal static SEntity GetRandomEnemy(Vector2 position)
        {
            return SEntityManager.Create(GetRandomEnemyType(), position);
        }

        private static Type GetRandomEnemyType()
        {
            return enemiesByDifficulty.Where(x => x.Key <= difficultyRate).SelectRandom().Value;
        }
    }
}
