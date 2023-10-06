using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Components;

namespace StardustDefender.Core.Camera
{
    public static class SCamera
    {
        public static Vector2 Position { get; set; }
        public static float Rotation { get; set; }
        public static float Zoom { get; set; }
        public static Vector2 Origin { get; set; }
        public static Vector2 Center => Position + Origin;

        internal static void Initialize()
        {
            Rotation = 0;
            Zoom = 2.5f;
            Origin = new(SScreen.Width / 2f, SScreen.Height / 2f);
            Position = Vector2.Zero;
        }

        public static void Move(Vector2 direction)
        {
            Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));
        }
        public static void Rotate(float deltaRadians)
        {
            Rotation += deltaRadians;
        }
        public static void LookAt(Vector2 position)
        {
            Position = position - new Vector2(SScreen.Width / 2f, SScreen.Height / 2f);
        }

        public static void ZoomIn(float deltaZoom)
        {
            Zoom += deltaZoom;
        }
        public static void ZoomOut(float deltaZoom)
        {
            Zoom -= deltaZoom;
        }

        public static Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }
        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = SScreen.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }
        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = SScreen.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y),
                   Matrix.Invert(GetViewMatrix()));
        }

        public static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }
        public static Matrix GetInverseViewMatrix()
        {
            return Matrix.Invert(GetViewMatrix());
        }

        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-Position.X, Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new(Origin, 0.0f));
        }
        private static Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, SScreen.Width, SScreen.Height, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        internal static BoundingFrustum GetBoundingFrustum()
        {
            Matrix viewMatrix = GetVirtualViewMatrix();
            Matrix projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new(projectionMatrix);
        }
        public static ContainmentType Contains(Point point)
        {
            return Contains(point.ToVector2());
        }
        public static ContainmentType Contains(Vector2 vector2)
        {
            return GetBoundingFrustum().Contains(new Vector3(vector2.X, vector2.Y, 0));
        }
        public static ContainmentType Contains(Rectangle rectangle)
        {
            Vector3 max = new(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            Vector3 min = new(rectangle.X, rectangle.Y, 0.5f);
            BoundingBox boundingBox = new(min, max);
            return GetBoundingFrustum().Contains(boundingBox);
        }
    }
}
