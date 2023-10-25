namespace StardustDefender.Core.Enums
{
    /// <summary>
    /// An enumeration representing the different states of the game.
    /// </summary>
    public enum SGameState
    {
        /// <summary>
        /// The game is in the introduction state.
        /// </summary>
        Introduction,

        /// <summary>
        /// The game is running in the normal state.
        /// </summary>
        Running,

        /// <summary>
        /// The game is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The player has achieved victory.
        /// </summary>
        Victory,

        /// <summary>
        /// The game is over, and the player has lost.
        /// </summary>
        GameOver,
    }
}
