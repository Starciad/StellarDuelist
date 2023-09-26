using Microsoft.Xna.Framework;

using StardustDefender.Camera;
using StardustDefender.Enums;
using StardustDefender.Entities.Player;
using StardustDefender.GUI.Common;
using StardustDefender.Managers;
using StardustDefender.World;

using System;
using System.Diagnostics;

namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
        internal static SPlayerEntity Player => player;
        internal static int PlayerCumulativeDamage => playerCumulativeDamage;
        internal static int Level => level;

        // Consts
        private const int ENEMY_SPAWN_RANGE = 4;

        // Entities
        private static SPlayerEntity player;

        // Entities (Boss)
        private static Type bossTypeSelected;
        private static bool bossDead;

        // Position
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

        // GUIs
        private static SGUIGameOver GUI_GameOver;

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
            GUI_GameOver = SGUIManager.Get<SGUIGameOver>();
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