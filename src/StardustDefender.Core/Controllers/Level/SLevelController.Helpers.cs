namespace StardustDefender.Controllers
{
    public static partial class SLevelController
    {
        public static void GameOver()
        {
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
            bossDead = true;
        }
    }
}
