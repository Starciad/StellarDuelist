using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Register;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;

namespace StellarDuelist.Core.Controllers
{
    /// <summary>
    /// Manages the difficulty of the game.
    /// </summary>
    public static partial class SDifficultyController
    {
        /// <summary>
        /// Gets the current difficulty rate.
        /// </summary>
        public static float DifficultyRate => difficultyRate;

        /// <summary>
        /// Gets the total number of enemies to be spawned.
        /// </summary>
        public static int TotalEnemyCount => totalEnemyCount;

        /// <summary>
        /// Gets the delay for enemy spawn.
        /// </summary>
        internal static float EnemySpawnDelay => enemySpawnDelay + SRandom.NextFloat();

        private static float difficultyRate = 0;
        private static float enemySpawnDelay = 0;
        private static int totalEnemyCount = 0;

        /// <summary>
        /// Initializes the difficulty controller.
        /// </summary>
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

        /// <summary>
        /// Begins the execution of the difficulty controller.
        /// </summary>
        internal static void BeginRun()
        {
            Reset();
        }

        /// <summary>
        /// Adjusts the game's difficulty based on player performance.
        /// </summary>
        internal static void Next()
        {
            // Easy
            if (SLevelController.PlayerCumulativeDamage <= 0)
            {
                difficultyRate += 1 + SRandom.NextFloat();
                enemySpawnDelay -= SRandom.Range(0, 2);
                totalEnemyCount += SRandom.Range(5, 11);
            }
            else // Hard
            {
                difficultyRate -= SRandom.NextFloat();
                enemySpawnDelay += SRandom.Range(0, 2) * SRandom.NextFloat();
                totalEnemyCount -= SRandom.Range(1, 7);
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

        /// <summary>
        /// Resets the difficulty settings to their initial values.
        /// </summary>
        internal static void Reset()
        {
            difficultyRate = 1f;
            enemySpawnDelay = 2.5f;
            totalEnemyCount = SRandom.Range(5, 11);
        }

        /// <summary>
        /// Gets a label indicating the current difficulty level.
        /// </summary>
        /// <returns>A string indicating the difficulty level.</returns>
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