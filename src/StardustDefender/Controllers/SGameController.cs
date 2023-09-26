using StardustDefender.Enums;
using StardustDefender.Managers;

using System;

namespace StardustDefender.Controllers
{


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
        internal static void Reset()
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