using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

using System;

namespace StardustDefender.Core.Controllers
{
    /// <summary>
    /// A static class responsible for managing the game's state and controlling its flow.
    /// </summary>
    public static class SGameController
    {
        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        public static SGameState State { get; private set; }

        /// <summary>
        /// Initializes the game and sets the initial state to Introduction.
        /// </summary>
        internal static void BeginRun()
        {
            SetGameState(SGameState.Introduction);
        }

        /// <summary>
        /// Sets the game state to the specified state.
        /// </summary>
        /// <param name="state">The new game state to set.</param>
        public static void SetGameState(SGameState state)
        {
            State = state;
        }

        /// <summary>
        /// Resets the game to its initial state, clearing controllers and managers.
        /// </summary>
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
