﻿using StardustDefender.Core;
using StardustDefender.Enums;

using Microsoft.Xna.Framework;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
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

        private static async Task RunLevelAsync()
        {
            // Wait until the number of enemies killed equals the total number of enemies in the level.
            while (enemiesKilled < SDifficultyController.TotalEnemyCount)
            {
                // Checks if the game ended prematurely.
                if (gameEnded)
                {
                    return;
                }

                // Wait until the game state is in "Running".
                while (SGameController.State != SGameState.Running)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                }

                // Spawns enemies if the number of spawned enemies is less than the total.
                if (spawnedEnemies < SDifficultyController.TotalEnemyCount)
                {
                    CreateEnemy();
                    spawnedEnemies++;
                }

                // Wait for a delay before creating the next enemy.
                if (SDifficultyController.EnemySpawnDelay > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(SDifficultyController.EnemySpawnDelay));
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1f));

            // Select a random boss and start a boss battle if possible.
            if (TrySelectingRandomBoss())
            {
                SFade.FadeIn(Color.White, 0.5f);
                await Task.Delay(TimeSpan.FromSeconds(1f));
                CreateBoss();
                await Task.Delay(TimeSpan.FromSeconds(1f));
                SFade.FadeOut(0.05f);

                // Wait until the boss is defeated.
                while (!bossDead)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                }

                // Wait for a delay after boss defeat.
                await Task.Delay(TimeSpan.FromSeconds(3.5f));
            }

            // Perform the level transition and advance the difficulty.
            await LevelTransitionAsync();
            SDifficultyController.Next();

            // Increments the level number and resets the level information.
            level++;
            ResetLevelInfos();

            // Call the function to perform the next level (recursion).
            RunLevel();

            // Returns a completed task.
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
    }
}