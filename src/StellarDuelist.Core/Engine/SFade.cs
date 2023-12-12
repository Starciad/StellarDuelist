using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Camera;
using StellarDuelist.Core.Extensions;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// Utility for controlling the game's universal fade system.
    /// </summary>
    public static class SFade
    {
        private static Texture2D _fadeTexture;
        private static Vector2 _position;
        private static Vector2 _origin;
        private static Color _color;

        private static Color currentColor;
        private static float fadeSmoothing = 0.2f;

        /// <summary>
        /// Load standard information for the execution of the Fade system.
        /// </summary>
        internal static void Load()
        {
            _fadeTexture = STextures.GetTexture("UI_SolidBackground");
            _position = SCamera.Center;
            _origin = _fadeTexture.GetOriginPosition();
            _color = Color.Transparent;
        }

        /// <summary>
        /// Update the Fade system.
        /// </summary>
        /// <remarks>
        /// This function is particularly useful because the fade system operates in a single-threaded manner, requiring it to update on each frame to quickly and practically apply solid color changes as specified.
        /// </remarks>
        internal static void Update()
        {
            currentColor = Color.Lerp(currentColor, _color, fadeSmoothing);
        }

        /// <summary>
        /// Render the Fade system.
        /// </summary>
        /// <remarks>
        /// Draws the current solid color of the Fade system on the screen.
        /// </remarks>
        internal static void Draw()
        {
            SGraphics.SpriteBatch.Draw(_fadeTexture, _position, null, currentColor, 0f, _origin, 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Gradually transition the Fade system to the specified solid color.
        /// </summary>
        /// <remarks>
        /// Based on the <paramref name="smoothing" /> value, a gradual color change is defined from the current fade system color to the color specified in <paramref name="color" />.
        /// </remarks>
        /// <param name="color">The new color to be gradually displayed.</param>
        /// <param name="smoothing">The smoothing value for the speed of color transition from the current color to the color specified in <paramref name="color" />.</param>
        public static void TransitionToColor(Color color, float smoothing)
        {
            _color = color;
            fadeSmoothing = smoothing;
        }
    }
}
