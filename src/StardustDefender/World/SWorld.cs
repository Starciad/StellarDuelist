using Microsoft.Xna.Framework;

using StardustDefender.Camera;

using System;

namespace StardustDefender.World
{
    internal static partial class SWorld
    {
        internal const float GridScale = 32;
        internal const float SmoothScale = 0.2f;

        internal const int Width = 7;
        internal const int Height = 12;

        internal static Vector2 Clamp(Vector2 pos)
        {
            Vector2 cameraCenter = GetLocalPosition(SCamera.Center);
            float leftBorder = cameraCenter.X - Width;
            float rightBorder = cameraCenter.X + Width;

            return new(Math.Clamp(pos.X, leftBorder, rightBorder), pos.Y);
        }

        internal static Vector2 GetWorldPosition(Vector2 pos)
        {
            return new(pos.X * GridScale, pos.Y * GridScale);
        }
        internal static Vector2 GetLocalPosition(Vector2 pos)
        {
            return new((float)Math.Round(pos.X / GridScale), (float)Math.Round(pos.Y / GridScale));
        }

        internal static bool InsideTheWorldDimensions(Vector2 pos)
        {
            return pos.X >= 0 && pos.X < Width &&
                   pos.Y >= 0 && pos.Y < Height;
        }
    }
}
