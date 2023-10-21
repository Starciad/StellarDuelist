using Microsoft.Xna.Framework.Graphics;

namespace StardustDefender.Core.Components
{
    /// <summary>
    /// A utility class for managing screen-related properties and settings.
    /// </summary>
    public static class SScreen
    {
        /// <summary>
        /// Gets the width of the screen in pixels.
        /// </summary>
        public static int Width => 1280;

        /// <summary>
        /// Gets the height of the screen in pixels.
        /// </summary>
        public static int Height => 720;

        /// <summary>
        /// Gets or sets the current viewport used for rendering.
        /// </summary>
        public static Viewport Viewport { get; private set; }

        /// <summary>
        /// Initializes the screen with the specified viewport.
        /// </summary>
        /// <param name="value">The viewport to be set as the current viewport.</param>
        internal static void Initialize(Viewport value)
        {
            Viewport = value;
        }
    }
}
