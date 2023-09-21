using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustDefender.Extensions
{
    internal static class Texture2DExtensions
    {
        internal static Vector2 GetOriginPosition(this Texture2D texture)
        {
            if (texture == null)
                return Vector2.Zero;

            return new(texture.Width / 2, texture.Height / 2);
        }
    }
}
