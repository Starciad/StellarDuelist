using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Engine;

namespace StellarDuelist.Core.Camera
{
    /// <summary>
    /// Manages the camera's position, rotation, and zoom for rendering.
    /// </summary>
    public static class SCamera
    {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public static Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle of the camera.
        /// </summary>
        public static float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the zoom level of the camera.
        /// </summary>
        public static float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the origin point of the camera.
        /// </summary>
        public static Vector2 Origin { get; set; }

        /// <summary>
        /// Gets the center of the camera.
        /// </summary>
        public static Vector2 Center => Position + Origin;

        /// <summary>
        /// Initializes the camera with default settings.
        /// </summary>
        internal static void Initialize()
        {
            Rotation = 0;
            Zoom = 2.5f;
            Origin = new Vector2(SScreen.Width / 2f, SScreen.Height / 2f);
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Moves the camera in a specified direction.
        /// </summary>
        /// <param name="direction">The direction to move the camera.</param>
        public static void Move(Vector2 direction)
        {
            Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));
        }

        /// <summary>
        /// Rotates the camera by a specified angle in radians.
        /// </summary>
        /// <param name="deltaRadians">The angle in radians to rotate the camera.</param>
        public static void Rotate(float deltaRadians)
        {
            Rotation += deltaRadians;
        }

        /// <summary>
        /// Sets the camera's position to look at a specific point.
        /// </summary>
        /// <param name="position">The point to look at.</param>
        public static void LookAt(Vector2 position)
        {
            Position = position - new Vector2(SScreen.Width / 2f, SScreen.Height / 2f);
        }

        /// <summary>
        /// Zooms in or out by a specified amount.
        /// </summary>
        /// <param name="deltaZoom">The amount to change the zoom level.</param>
        public static void ZoomIn(float deltaZoom)
        {
            Zoom += deltaZoom;
        }

        /// <summary>
        /// Zooms in or out by a specified amount.
        /// </summary>
        /// <param name="deltaZoom">The amount to change the zoom level.</param>
        public static void ZoomOut(float deltaZoom)
        {
            Zoom -= deltaZoom;
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates for a specified point.
        /// </summary>
        /// <param name="x">The X-coordinate of the point in world space.</param>
        /// <param name="y">The Y-coordinate of the point in world space.</param>
        /// <returns>The screen coordinates of the point.</returns>
        public static Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }

        /// <summary>
        /// Converts world coordinates to screen coordinates for a specified vector.
        /// </summary>
        /// <param name="worldPosition">The vector representing the point in world space.</param>
        /// <returns>The screen coordinates of the point.</returns>
        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = SScreen.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        /// <summary>
        /// Converts screen coordinates to world coordinates for a specified vector.
        /// </summary>
        /// <param name="screenPosition">The vector representing the point in screen space.</param>
        /// <returns>The world coordinates of the point.</returns>
        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = SScreen.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        /// <summary>
        /// Gets the view matrix used for rendering.
        /// </summary>
        /// <returns>The view matrix.</returns>
        public static Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        /// <summary>
        /// Gets the inverse of the view matrix.
        /// </summary>
        /// <returns>The inverse view matrix.</returns>
        public static Matrix GetInverseViewMatrix()
        {
            return Matrix.Invert(GetViewMatrix());
        }

        /// <summary>
        /// Gets the virtual view matrix for rendering.
        /// </summary>
        /// <returns>The virtual view matrix.</returns>
        private static Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        /// <summary>
        /// Gets the projection matrix for rendering.
        /// </summary>
        /// <param name="viewMatrix">The view matrix used for rendering.</param>
        /// <returns>The projection matrix.</returns>
        private static Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, SScreen.Width, SScreen.Height, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        /// <summary>
        /// Gets the bounding frustum used for culling.
        /// </summary>
        /// <returns>The bounding frustum.</returns>
        internal static BoundingFrustum GetBoundingFrustum()
        {
            Matrix viewMatrix = GetVirtualViewMatrix();
            Matrix projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new BoundingFrustum(projectionMatrix);
        }

        /// <summary>
        /// Determines if a point is within the camera's view frustum.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>The containment type of the point.</returns>
        public static ContainmentType Contains(Point point)
        {
            return Contains(point.ToVector2());
        }

        /// <summary>
        /// Determines if a vector is within the camera's view frustum.
        /// </summary>
        /// <param name="vector2">The vector to check for containment.</param>
        /// <returns>The containment type of the vector.</returns>
        public static ContainmentType Contains(Vector2 vector2)
        {
            return GetBoundingFrustum().Contains(new Vector3(vector2.X, vector2.Y, 0));
        }

        /// <summary>
        /// Determines if a rectangle is within the camera's view frustum.
        /// </summary>
        /// <param name="rectangle">The rectangle to check for containment.</param>
        /// <returns>The containment type of the rectangle.</returns>
        public static ContainmentType Contains(Rectangle rectangle)
        {
            Vector3 max = new(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0.5f);
            Vector3 min = new(rectangle.X, rectangle.Y, 0.5f);
            BoundingBox boundingBox = new(min, max);
            return GetBoundingFrustum().Contains(boundingBox);
        }
    }
}
