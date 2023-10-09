using Microsoft.Xna.Framework;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.World;

using System;
using System.Diagnostics;

namespace StardustDefender.Controllers
{
    public static partial class SLevelController
    {
        public static SPlayerEntity Player => player;
        public static int Level => level;
        public static TimeSpan TotalGameTime => totalGameTime.Elapsed;
        internal static int PlayerCumulativeDamage => playerCumulativeDamage;
        public static bool BossIncoming => bossIncoming;
        public static bool BossAppeared => bossAppeared;

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
        private static bool initialized;
        private static bool gameEnded;

        // Timers
        private static readonly Stopwatch totalGameTime = new();

        // Events
        public static event GameFinished OnGameFinished;
        public delegate void GameFinished();

        internal static void Initialize()
        {
            centerPosition = SWorld.GetLocalPosition(SCamera.Center);
            playerPosition = new(centerPosition.X, centerPosition.Y + 4);
            enemyPosition = new(centerPosition.X, centerPosition.Y - 5);
            bossPosition = new(centerPosition.X - 0.5f, centerPosition.Y - 2f);

            minEntityDespawnLimit = new(centerPosition.X - 10, centerPosition.Y - 5);
            maxEntityDespawnLimit = new(centerPosition.X + 10, centerPosition.Y + 5);
        }
        internal static void BeginRun()
        {
            SGameController.SetGameState(SGameState.Introduction);
        }
        internal static void Update()
        {
            RemoveObjectsOffScreen();
        }
        internal static void Reset()
        {
            level = 0;

            enemiesKilled = 0;
            spawnedEnemies = 0;
            playerCumulativeDamage = 0;

            initialized = false;
        }
    }
}