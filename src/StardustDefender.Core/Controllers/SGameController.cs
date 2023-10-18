using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

using System;

namespace StardustDefender.Core.Controllers
{
    public static class SGameController
    {
        public static SGameState State { get; private set; }

        internal static void BeginRun()
        {
            SetGameState(SGameState.Introduction);
        }
        public static void SetGameState(SGameState state)
        {
            State = state;
        }

        public static void Reset()
        {
            SetGameState(SGameState.Introduction);

            // Controllers
            SBackgroundController.Reset();
            SDifficultyController.Reset();
            SLevelController.Reset();

            // Managers
            SEffectsManager.Reset();
            SEntityManager.Reset();
            SItemsManager.Reset();
            SProjectileManager.Reset();

            GC.Collect();
        }
    }
}