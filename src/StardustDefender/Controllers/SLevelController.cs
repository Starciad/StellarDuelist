using Microsoft.Xna.Framework;

using StardustDefender.Camera;
using StardustDefender.Engine;
using StardustDefender.Entities;
using StardustDefender.Entities.Bosses;
using StardustDefender.Entities.Player;
using StardustDefender.GUI.Common;
using StardustDefender.Items;
using StardustDefender.Managers;
using StardustDefender.World;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
        internal static SPlayerEntity Player => player;
        internal static int PlayerCumulativeDamage => playerCumulativeDamage;
        internal static int Level => level;

        // Entities
        private static SPlayerEntity player;
        private static SBossEntity boss;

        // Consts
        private const int ENEMY_SPAWN_RANGE = 4;

        // Position
        private static Vector2 centerPosition;
        private static Vector2 playerPosition;
        private static Vector2 enemyPosition;
        private static Vector2 bossPosition;

        private static Vector2 minDespawnLimit;
        private static Vector2 maxDespawnLimit;

        // Counters
        private static int level = 0;
        private static int enemiesKilled = 0;
        private static int spawnedEnemies = 0;
        private static int playerCumulativeDamage = 0;

        // States
        private static bool bossDead;
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
            bossPosition = new(centerPosition.X, centerPosition.Y - 2.5f);

            minDespawnLimit = new(centerPosition.X - 10, centerPosition.Y - 5);
            maxDespawnLimit = new(centerPosition.X + 10, centerPosition.Y + 5);
        }
        internal static void BeginRun()
        {
            GUI_GameOver = SGUIManager.Get<SGUIGameOver>();
            SGameController.SetGameState(SGameState.Introduction);
        }
        internal static void Update()
        {
            RemoveAllOffScreenElements();
        }
        internal static void Reset()
        {
            level = 0;

            enemiesKilled = 0;
            spawnedEnemies = 0;
            playerCumulativeDamage = 0;

            initialized = false;
        }

        private static void RemoveAllOffScreenElements()
        {
            // Entities
            foreach (SEntity entity in SEntityManager.Entities)
            {
                if (entity == null)
                {
                    continue;
                }

                if (entity.LocalPosition.X < minDespawnLimit.X ||
                    entity.LocalPosition.Y < minDespawnLimit.Y ||
                    entity.LocalPosition.X > maxDespawnLimit.X ||
                    entity.LocalPosition.Y > maxDespawnLimit.Y)
                {
                    if (entity is SPlayerEntity)
                    {
                        continue;
                    }

                    spawnedEnemies--;
                    SEntityManager.Remove(entity);
                }
            }

            // Items
            foreach (SItem item in SItemsManager.Items)
            {
                if (item.Position.X < minDespawnLimit.X * SWorld.GridScale ||
                    item.Position.Y < minDespawnLimit.Y * SWorld.GridScale ||
                    item.Position.X > maxDespawnLimit.X * SWorld.GridScale ||
                    item.Position.Y > maxDespawnLimit.Y * SWorld.GridScale)
                {
                    SItemsManager.Remove(item);
                }
            }
        }

        internal static void RunLevel()
        {
            if (!initialized)
            {
                totalGameTime.Restart();
                CreatePlayer();

                initialized = true;
                gameEnded = false;
            }

            _ = Task.Run(RunLevelAsync);
        }
        internal static void GameOver()
        {
            gameEnded = true;
            initialized = false;

            totalGameTime.Stop();
            GUI_GameOver.Build(totalGameTime.Elapsed, level + 1);
        }

        internal static void PlayerDamaged(int value)
        {
            playerCumulativeDamage += value;
        }
        internal static void EnemyKilled()
        {
            enemiesKilled++;
        }
        internal static void BossKilled()
        {
            bossDead = true;
            boss = null;
        }

        private static async Task RunLevelAsync()
        {
            // Game
            while (enemiesKilled < SDifficultyController.TotalEnemyCount)
            {
                if (spawnedEnemies < SDifficultyController.TotalEnemyCount)
                {
                    CreateEnemy();
                    spawnedEnemies++;
                }

                if (gameEnded) { return; }

                if (SDifficultyController.EnemySpawnDelay > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(SDifficultyController.EnemySpawnDelay));
                }

                if (gameEnded) { return; }
            }

            await Task.Delay(TimeSpan.FromSeconds(1f));

            // Boss
            if (TryCreateBoss())
            {
                SFade.FadeIn(Color.White, 0.5f);
                await Task.Delay(TimeSpan.FromSeconds(1.5f));
                SFade.FadeOut(0.05f);

                while (!bossDead)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                }

                await Task.Delay(TimeSpan.FromSeconds(3.5f));
            }

            // Win
            await LevelTransitionAsync();
            SDifficultyController.Next();

            // Reset
            level++;
            ResetLevelInfos();

            // Next level (Loop)
            RunLevel();
            await Task.CompletedTask;
        }
        private static async Task LevelTransitionAsync()
        {
            for (int i = 0; i < 20; i++)
            {
                SBackgroundController.GlobalParallaxFactor += 1.5f;
                Player.LocalPosition = new(Player.LocalPosition.X, Player.LocalPosition.Y - 1f);

                await Task.Delay(250);
            }

            SFade.FadeIn(Color.White, 0.5f);
            await Task.Delay(TimeSpan.FromSeconds(1f));
            SBackgroundController.GlobalParallaxFactor = 1;
            ResetPlayerPosition();
            await Task.Delay(TimeSpan.FromSeconds(2f));
            SFade.FadeOut(0.05f);
        }

        private static void CreatePlayer()
        {
            player = SEntityManager.Create<SPlayerEntity>(playerPosition);
            ResetPlayerPosition();
        }
        private static void CreateEnemy()
        {
            _ = SDifficultyController.CreateRandomEnemy(new(enemyPosition.X + SRandom.Range(-ENEMY_SPAWN_RANGE, ENEMY_SPAWN_RANGE + 1), enemyPosition.Y));
        }
        private static bool TryCreateBoss()
        {
            if (SDifficultyController.TryCreateRandomBoss(bossPosition, out SBossEntity value))
            {
                boss = value;
                bossDead = false;

                return true;
            }

            return false;
        }

        private static void ResetPlayerPosition()
        {
            Player.LocalPosition = playerPosition;
            Player.WorldPosition += SWorld.GetWorldPosition(new(0, 10));
        }
        private static void ResetLevelInfos()
        {
            enemiesKilled = 0;
            spawnedEnemies = 0;
            playerCumulativeDamage = 0;
        }
    }
}