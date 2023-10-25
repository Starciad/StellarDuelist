using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Managers;
using StardustDefender.Core.Enums;

namespace StardustDefender.Controllers
{
    public static partial class SDifficultyController
    {
        public static float DifficultyRate => difficultyRate;
        internal static float EnemySpawnDelay => enemySpawnDelay + SRandom.NextFloat();
        internal static int TotalEnemyCount => totalEnemyCount;

        private static float difficultyRate = 0;
        private static float enemySpawnDelay = 0;
        private static int totalEnemyCount = 0;

        internal static void Initialize()
        {
            foreach (SEntityHeader header in SEntityManager.EntityHeaders)
            {
                switch (header.Classification)
                {
                    case SEntityClassification.None:
                        continue;

                    case SEntityClassification.Enemy:
                        enemies.Add(header);
                        continue;

                    case SEntityClassification.Boss:
                        remainingBosses.Add(header);
                        continue;

                    default:
                        continue;
                }
            }

            // Setting bosses
            allBosses = remainingBosses.ToArray();
        }
        internal static void BeginRun()
        {
            Reset();
        }
        internal static void Next()
        {
            // Easy
            if (SLevelController.PlayerCumulativeDamage <= 0)
            {
                difficultyRate += 1 + SRandom.NextFloat();
                enemySpawnDelay -= SRandom.Range(0, 2) * SRandom.NextFloat();
                totalEnemyCount += SRandom.Range(4, 9);
            }
            else // Hard
            {
                difficultyRate -= SRandom.NextFloat();
                enemySpawnDelay += SRandom.Range(0, 2) * SRandom.NextFloat();
                totalEnemyCount -= SRandom.Range(3, 7);
            }

            if (difficultyRate < 0)
            {
                difficultyRate = 0;
            }

            if (totalEnemyCount < 5)
            {
                totalEnemyCount = 5;
            }

            if (enemySpawnDelay < 0)
            {
                enemySpawnDelay = 0;
            }
        }
        internal static void Reset()
        {
            difficultyRate = 1f;
            enemySpawnDelay = 2.5f;
            totalEnemyCount = SRandom.Range(5, 11);
        }

        public static string GetDifficultyLabel()
        {
            return difficultyRate switch
            {
                <= 1 => "VERY EASY",
                <= 2 => "EASY",
                <= 4 => "NORMAL",
                <= 8 => "UNUSUAL",
                <= 16 => "HARD",
                <= 32 => "VERY HARD",
                _ => "ULTRA+",
            };
        }
    }
}