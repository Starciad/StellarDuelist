using Microsoft.Xna.Framework;

using StellarDuelist.Core.Camera;

using System;

namespace StellarDuelist.Core.World
{
    /// <summary>
    /// Static class for managing world-related calculations and transformations.
    /// </summary>
    public static partial class SWorld
    {
        /// <summary>
        /// The scale of the grid within the world.
        /// </summary>
        internal const float GridScale = 32;

        /// <summary>
        /// The smooth scaling factor used in calculations.
        /// </summary>
        internal const float SmoothScale = 0.2f;

        /// <summary>
        /// The width of the world.
        /// </summary>
        internal const int Width = 4;

        /// <summary>
        /// The height of the world.
        /// </summary>
        internal const int Height = 12;

        /// <summary>
        /// Fixes a position within the horizontal boundaries of the world.
        /// </summary>
        /// <param name="pos">The position to clamp.</param>
        /// <returns>The clamped position.</returns>
        public static Vector2 ClampHorizontalPosition(Vector2 pos)
        {
            Vector2 cameraCenter = GetLocalPosition(SCamera.Center);
            float leftBorder = cameraCenter.X - Width;
            float rightBorder = cameraCenter.X + Width;

            return new Vector2(Math.Clamp(pos.X, leftBorder, rightBorder), pos.Y);
        }

        /// <summary>
        /// Fixes a position within the vertical limits of the world.
        /// </summary>
        /// <param name="pos">The position to clamp.</param>
        /// <returns>The clamped position.</returns>
        public static Vector2 ClampVerticalPosition(Vector2 pos)
        {
            Vector2 cameraCenter = GetLocalPosition(SCamera.Center);

            return new Vector2(pos.X, Math.Clamp(pos.Y, cameraCenter.Y - 3, cameraCenter.Y + 4));
        }

        /// <summary>
        /// Converts a local position to a world position based on the grid scale.
        /// </summary>
        /// <param name="pos">The local position to convert.</param>
        /// <returns>The corresponding world position.</returns>
        public static Vector2 GetWorldPosition(Vector2 pos)
        {
            return new Vector2(pos.X * GridScale, pos.Y * GridScale);
        }

        /// <summary>
        /// Converts a world position to a local position based on the grid scale.
        /// </summary>
        /// <param name="pos">The world position to convert.</param>
        /// <returns>The corresponding local position.</returns>
        public static Vector2 GetLocalPosition(Vector2 pos)
        {
            return new Vector2((float)(pos.X / GridScale), (float)(pos.Y / GridScale));
        }

        /// <summary>
        /// Checks if a position is inside the dimensions of the world.
        /// </summary>
        /// <param name="pos">The position to check.</param>
        /// <returns>True if the position is inside the world dimensions; otherwise, false.</returns>
        public static bool InsideTheWorldDimensions(Vector2 pos)
        {
            return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
        }
    }
}
