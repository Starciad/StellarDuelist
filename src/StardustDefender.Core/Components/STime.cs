using Microsoft.Xna.Framework;

namespace StardustDefender.Core.Components
{
    /// <summary>
    /// Static utility class for managing game time information.
    /// </summary>
    public static class STime
    {
        /// <summary>
        /// Gets the GameTime object for update time.
        /// </summary>
        public static GameTime UpdateTime { get; private set; }

        /// <summary>
        /// Gets the GameTime object for draw time.
        /// </summary>
        public static GameTime DrawTime { get; private set; }

        /// <summary>
        /// Sets the GameTime object for update time.
        /// </summary>
        /// <param name="value">The GameTime object for update time.</param>
        internal static void SetUpdateGameTime(GameTime value)
        {
            UpdateTime = value;
        }

        /// <summary>
        /// Sets the GameTime object for draw time.
        /// </summary>
        /// <param name="value">The GameTime object for draw time.</param>
        internal static void SetDrawGameTime(GameTime value)
        {
            DrawTime = value;
        }
    }
}
