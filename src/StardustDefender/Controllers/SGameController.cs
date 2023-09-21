namespace StardustDefender.Controllers
{
    internal enum SGameState
    {
        Introduction,
        Running,
        Paused,
        Victory,
        GameOver
    }

    internal static class SGameController
    {
        internal static SGameState State { get; private set; }

        internal static void BeginRun()
        {
            SetGameState(SGameState.Introduction);
        }
        internal static void SetGameState(SGameState state)
        {
            State = state;
        }
    }
}
