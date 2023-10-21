using Microsoft.Xna.Framework;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.World;

using System;
using System.Diagnostics;

namespace StardustDefender.Core.Controllers
{
    /// <summary>
    /// A static class responsible for managing game levels, including player progress, enemies, and bosses.
    /// </summary>
    public static partial class SLevelController
    {
        /// <summary>
        /// Gets the player entity currently active in the level.
        /// </summary>
        public static SPlayerEntity Player => player;

        /// <summary>
        /// Gets the current level number.
        /// </summary>
        public static int Level => level;

        /// <summary>
        /// Gets the total game time elapsed during the level.
        /// </summary>
        public static TimeSpan TotalGameTime => totalGameTime.Elapsed;

        /// <summary>
        /// Gets the cumulative damage received by the player during the level.
        /// </summary>
        internal static int PlayerCumulativeDamage => playerCumulativeDamage;

        /// <summary>
        /// Gets a value indicating whether a boss is incoming in the level.
        /// </summary>
        public static bool BossIncoming => bossIncoming;

        /// <summary>
        /// Gets a value indicating whether a boss has appeared in the level.
        /// </summary>
        public static bool BossAppeared => bossAppeared;

        /// <summary>
        /// Gets the number of enemies killed by the player in the level.
        /// </summary>
        public static int EnemiesKilled => enemiesKilled;

        // ======================================= //
        // Consts
        private const int ENEMY_SPAWN_RANGE = 4;

        // Entities
        private static SPlayerEntity player;

        // Entities (Boss)
        private static Type bossTypeSelected;
        private static bool bossIncoming;
        private static bool bossAppeared;
        private static bool bossDead;

        // Positions
        private static Vector2 centerPosition;
        private static Vector2 playerPosition;
        private static Vector2 enemyPosition;
        private static Vector2 bossPosition;

        private static Vector2 minEntityDespawnLimit;
        private static Vector2 maxEntityDespawnLimit;

        // Counters
        private static int level = 0;
        private static int enemiesKilled = 0;
        private static int spawnedEnemies = 0;
        private static int playerCumulativeDamage = 0;

        // States
        private static bool levelInitialized;
        private static bool gameEnded;

        // Timers
        private static readonly Stopwatch totalGameTime = new();

        // ======================================= //
        // Events

        /// <summary>
        /// Occurs when the game level is finished, such as when the player wins or loses.
        /// </summary>
        public static event GameFinished OnGameFinished;

        public delegate void GameFinished();

        /// <summary>
        /// Initializes the level controller with initial positions and limits.
        /// </summary>
        internal static void Initialize()
        {
            centerPosition = SWorld.GetLocalPosition(SCamera.Center);
            playerPosition = new(centerPosition.X, centerPosition.Y + 4);
            enemyPosition = new(centerPosition.X, centerPosition.Y - 5);
            bossPosition = new(centerPosition.X - 0.5f, centerPosition.Y - 2f);

            minEntityDespawnLimit = new(centerPosition.X - 10, centerPosition.Y - 5);
            maxEntityDespawnLimit = new(centerPosition.X + 10, centerPosition.Y + 5);
        }

        /// <summary>
        /// Begins the execution of the level.
        /// </summary>
        internal static void BeginRun()
        {
            SGameController.SetGameState(SGameState.Introduction);
        }

        /// <summary>
        /// Updates the level components.
        /// </summary>
        internal static void Update()
        {
            RemoveObjectsOffScreen();
        }

        /// <summary>
        /// Resets the level to its initial state.
        /// </summary>
        internal static void Reset()
        {
            level = 0;
            enemiesKilled = 0;
            spawnedEnemies = 0;
            playerCumulativeDamage = 0;
            levelInitialized = false;
        }

    }
}