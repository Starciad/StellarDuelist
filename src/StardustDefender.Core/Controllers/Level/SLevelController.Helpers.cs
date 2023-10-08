using StardustDefender.Core.Components;

namespace StardustDefender.Controllers
{
    public static partial class SLevelController
    {
        public static void GameOver()
        {
            SSongs.Play($"Game_Over_{SRandom.Range(1, 4)}");

            gameEnded = true;
            initialized = false;

            totalGameTime.Stop();
            OnGameFinished?.Invoke();
        }

        public static void PlayerDamaged(int value)
        {
            playerCumulativeDamage += value;
        }

        public static void EnemyKilled()
        {
            enemiesKilled++;
        }

        public static void BossKilled()
        {
            SSongs.Play($"Victory_Boss");
            bossDead = true;
        }
    }
}
