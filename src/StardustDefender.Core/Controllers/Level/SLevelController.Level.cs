using Microsoft.Xna.Framework;

using StardustDefender.Core.Components;
using StardustDefender.Core.Enums;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Core.Controllers
{
    public static partial class SLevelController
    {
        /// <summary>
        /// Starts a new game level, initializing necessary components and routines.
        /// </summary>
        public static void StartNewLevel()
        {
            // Checks if the level is initialized, if it is false, the first components are created/initialized.
            if (!levelInitialized)
            {
                totalGameTime.Restart();
                CreatePlayer();

                levelInitialized = true;
                gameEnded = false;
            }

            // Reset the player's position and initialize the level's asynchronous routine.
            ResetPlayerPosition();
            _ = Task.Run(InitializeLevelRoutineAsync);
        }

        private static async Task InitializeLevelRoutineAsync()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // Wait for the game to be active.
            await WaitForActivityAsync();

            // All of the methods below make up a level progression in StardustDefender. Each of them has specific descriptions of what they are and what they do.
            await Level_SelectRandomBoss_Async();
            await Level_WaveOfEnemies_Async();
            await Level_BossBattle_Async();
            await Level_Victory_Async();
            await Level_Finishing_Async();

            // Task completion.
            await Task.CompletedTask;
        }
        private static async Task Level_SelectRandomBoss_Async()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // At the beginning of the level, try to select a random boss.
            // If so, the `bossIncoming` variable is marked as `true` and tense music is chosen for the level's theme.
            // If not, the `bossIncoming` variable is marked as `false` and a normal song is chosen for the level's theme.
            if (TrySelectingRandomBoss())
            {
                bossIncoming = true;
                SSongs.Play($"Boss_Incoming_{SRandom.Range(1, 6)}");
            }
            else
            {
                bossIncoming = false;
                SSongs.Play($"Area_{SRandom.Range(1, 17)}");
            }

            // Task completion.
            await Task.CompletedTask;
        }
        private static async Task Level_WaveOfEnemies_Async()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // Wait until the number of enemies killed equals the total number of enemies in the level.
            while (enemiesKilled < SDifficultyController.TotalEnemyCount)
            {
                // Checks if the game ended prematurely.
                if (gameEnded)
                {
                    return;
                }

                await WaitForActivityAsync();

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

            // Task completion.
            await Task.CompletedTask;
        }
        private static async Task Level_BossBattle_Async()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // Start a boss battle if possible.
            if (bossIncoming)
            {
                // Activate information that the boss has appeared and stop the music.
                bossAppeared = true;
                CleanProjectiles();

                // Stop music and start fade.
                SSongs.Stop();
                SFade.TransitionToColor(Color.White, 0.5f);
                await Task.Delay(TimeSpan.FromSeconds(1f));

                // Wait for the game to be in the Running state if it is not about to create the boss.
                await WaitForActivityAsync();
                CreateBoss();

                // Make fade disappear and start boss theme song.
                await Task.Delay(TimeSpan.FromSeconds(1f));
                SFade.TransitionToColor(Color.Transparent, 0.05f);
                SSongs.Play($"Boss_{SRandom.Range(1, 6)}");

                // Wait until the boss is defeated.
                while (!bossDead)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1f));
                }

                // Wait for a delay after boss defeat.
                await Task.Delay(TimeSpan.FromSeconds(10.5f));

                // Clearing boss variables.
                bossTypeSelected = null;
                bossAppeared = false;
                bossIncoming = false;
            }

            // Task completion.
            await Task.CompletedTask;
        }
        private static async Task Level_Victory_Async()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // If the current level does not have a boss, play standard victory music.
            if (!bossIncoming)
            {
                SSongs.Play($"Victory_{SRandom.Range(1, 4)}");
            }

            // Task completion.
            await Task.CompletedTask;
        }
        private static async Task Level_Finishing_Async()
        {
            // Check if the game is finished.
            if (gameEnded)
            {
                return;
            }

            // Perform the level transition and advance the difficulty.
            CleanProjectiles();
            await LevelTransitionAsync();
            SDifficultyController.Next();

            // Increments the level number and resets the level information.
            level++;
            ResetLevelInfos();

            // Call the function to perform the next level (recursion).
            StartNewLevel();

            // Task completion.
            await Task.CompletedTask;
        }

        // Utilities
        private static async Task LevelTransitionAsync()
        {
            // Increases the player's speed in conjunction with the background to give the impression that he is advancing to the next level.
            for (int i = 0; i < 25; i++)
            {
                await WaitForActivityAsync();

                SBackgroundController.GlobalParallaxFactor += 1.5f;

                await Task.Delay(250);
            }

            // Initializes the fade to cover the screen for a short interval.
            SFade.TransitionToColor(Color.White, 0.5f);
            await Task.Delay(TimeSpan.FromSeconds(1f));
            SBackgroundController.GlobalParallaxFactor = 1;
            ResetPlayerPosition();
            await WaitForActivityAsync();
            await Task.Delay(TimeSpan.FromSeconds(1.5f));
            SFade.TransitionToColor(Color.Transparent, 0.05f);
        }
        private static async Task WaitForActivityAsync()
        {
            // Wait until the game state is in "Running".
            while (SGameController.State != SGameState.Running ||
                   !SGame.Instance.IsActive)
            {
                await Task.Delay(TimeSpan.FromSeconds(1f));
            }
        }
    }
}
