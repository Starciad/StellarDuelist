using Microsoft.Xna.Framework;

using StardustDefender.Camera;
using StardustDefender.Core;
using StardustDefender.Entities;
using StardustDefender.Entities.Bosses;
using StardustDefender.Entities.Player;
using StardustDefender.GUI.Common;
using StardustDefender.Items;
using StardustDefender.Managers;
using StardustDefender.World;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

                if (entity.LocalPosition.X < minEntityDespawnLimit.X ||
                    entity.LocalPosition.Y < minEntityDespawnLimit.Y ||
                    entity.LocalPosition.X > maxEntityDespawnLimit.X ||
                    entity.LocalPosition.Y > maxEntityDespawnLimit.Y)
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
                if (item.Position.X < minEntityDespawnLimit.X * SWorld.GridScale ||
                    item.Position.Y < minEntityDespawnLimit.Y * SWorld.GridScale ||
                    item.Position.X > maxEntityDespawnLimit.X * SWorld.GridScale ||
                    item.Position.Y > maxEntityDespawnLimit.Y * SWorld.GridScale)
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
        }

        private static async Task RunLevelAsync()
        {
            #region LEVEL LOOP
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
            #endregion
            
            #region BOSS BATTLE
            if (TrySelectingRandomBoss())
            {
                SFade.FadeIn(Color.White, 0.5f);
                await Task.Delay(TimeSpan.FromSeconds(1f));
                CreateBoss();
                await Task.Delay(TimeSpan.FromSeconds(1f));
                SFade.FadeOut(0.05f);

                while (!bossDead)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                }

                await Task.Delay(TimeSpan.FromSeconds(3.5f));
            }
            #endregion

            #region VICTORY
            await LevelTransitionAsync();
            SDifficultyController.Next();
            #endregion

            #region LEVEL RESET
            level++;
            ResetLevelInfos();
            #endregion

            #region NEXT LEVEL (RECURSION)
            RunLevel();
            #endregion

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
        private static void CreateBoss()
        {
            if (bossTypeSelected == null)
                return;

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