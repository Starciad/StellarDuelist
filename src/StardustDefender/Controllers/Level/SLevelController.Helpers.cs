namespace StardustDefender.Controllers
{
    internal static partial class SLevelController
    {
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
    }
}
