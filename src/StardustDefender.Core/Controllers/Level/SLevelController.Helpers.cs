using StardustDefender.Core.Components;

namespace StardustDefender.Core.Controllers
{
    public static partial class SLevelController
    {
        /// <summary>
        /// Ends the game level, triggering game over and resetting level-specific data.
        /// </summary>
        public static void GameOver()
        {
            gameEnded = true;
            levelInitialized = false;

            totalGameTime.Stop();
            OnGameFinished?.Invoke();
        }

        /// <summary>
        /// Updates the cumulative damage received by the player in the level.
        /// </summary>
        /// <param name="value">The amount of damage received.</param>
        public static void PlayerDamaged(int value)
        {
            playerCumulativeDamage += value;
        }

        /// <summary>
        /// Updates the count of enemies killed by the player in the level.
        /// </summary>
        public static void EnemyKilled()
        {
            enemiesKilled++;
        }

        /// <summary>
        /// Marks the boss as killed, playing a victory sound.
        /// </summary>
        public static void BossKilled()
        {
            SSongs.Play($"Victory_Boss");
            bossDead = true;
        }
    }
}
