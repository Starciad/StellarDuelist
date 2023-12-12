using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StellarDuelist.Core.Extensions
{
    /// <summary>
    /// Provides extensions for Texture2D objects.
    /// </summary>
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Gets the origin position for a Texture2D object.
        /// </summary>
        /// <param name="texture">The Texture2D object for which to get the origin position.</param>
        /// <returns>The origin position as a Vector2.</returns>
        public static Vector2 GetOriginPosition(this Texture2D texture)
        {
            return texture == null ? Vector2.Zero : new Vector2(texture.Width / 2, texture.Height / 2);
        }
    }
}
