using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;

using System;

namespace StellarDuelist.Core.Controllers
{
    public struct DifficultySettings
    {
        /// <summary>
        /// Gets the delay for enemy spawn.
        /// </summary>
        public float EnemySpawnDelay { get; internal set; }

        /// <summary>
        /// Gets the total number of enemies to be spawned.
        /// </summary>
        public int TotalEnemyCount { get; internal set; }

        /// <summary>
        /// Gets the current difficulty rate.
        /// </summary>
        public float DifficultyRate { get; internal set; }

        internal void Clamp()
        {
            if (DifficultyRate < 0)
            {
                DifficultyRate = 0;
            }

            if (TotalEnemyCount < 5)
            {
                TotalEnemyCount = 5;
            }

            if (EnemySpawnDelay < 0)
            {
                EnemySpawnDelay = 0;
            }
        }
    }

    /// <summary>
    /// Manages the difficulty of the game.
    /// </summary>
    public static partial class SDifficultyController
    {
        public static DifficultySettings DifficultySettings => currentDifficultySettings;

        private static readonly DifficultySettings baseDifficultySettings = new()
        {
            EnemySpawnDelay = 2.5f,
            TotalEnemyCount = SRandom.Range(3, 7), // (3 - 6)
            DifficultyRate = 1f
        };

        private static DifficultySettings currentDifficultySettings;

        /// <summary>
        /// Initializes the difficulty controller.
        /// </summary>
        internal static void Initialize()
        {
            foreach (SEntityDefinition definition in SEntityManager.EntityDefinitions)
            {
                switch (definition.Classification)
                {
                    case SEntityClassification.None:
                        continue;

                    case SEntityClassification.Enemy:
                        enemies.Add(definition);
                        continue;

                    case SEntityClassification.Boss:
                        remainingBosses.Add(definition);
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
        internal static void Next(float playerPerformance)
        {
            float n_enemySpawnDelay = SRandom.NextFloat() / 2f;
            int n_totalEnemyCount = SRandom.Range(2, 5); // (2 - 4)
            float n_difficultyRate = SRandom.NextFloat() + SRandom.NextFloat();

            if (playerPerformance <= 0)
            {
                // Increase difficulty
                currentDifficultySettings.EnemySpawnDelay -= n_enemySpawnDelay / 2f;
                currentDifficultySettings.TotalEnemyCount += n_totalEnemyCount;
                currentDifficultySettings.DifficultyRate += n_difficultyRate;
            }
            else
            {
                // Decrease difficulty
                currentDifficultySettings.EnemySpawnDelay += n_enemySpawnDelay / 4f;
                currentDifficultySettings.TotalEnemyCount -= (int)Math.Round(n_totalEnemyCount / 2f);
                currentDifficultySettings.DifficultyRate -= n_difficultyRate / 4f;
            }

            currentDifficultySettings.Clamp();
        }

        /// <summary>
        /// Resets the difficulty settings to their initial values.
        /// </summary>
        internal static void Reset()
        {
            currentDifficultySettings = baseDifficultySettings;
        }

        /// <summary>
        /// Gets a label indicating the current difficulty level.
        /// </summary>
        /// <returns>A string indicating the difficulty level.</returns>
        public static string GetDifficultyLabel()
        {
            return currentDifficultySettings.DifficultyRate switch
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