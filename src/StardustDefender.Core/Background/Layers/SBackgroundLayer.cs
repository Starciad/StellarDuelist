using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;

namespace StardustDefender.Core.Background.Layers
{
    /// <summary>
    /// Represents a layer of the <see cref="SBackground"/> with parallax scrolling.
    /// </summary>
    internal sealed class SBackgroundLayer
    {
        /// <summary>
        /// Gets the texture used for the background layer.
        /// </summary>
        internal Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the rectangle that defines the portion of the texture to display.
        /// </summary>
        internal Rectangle TextureRectangle { get; private set; }

        /// <summary>
        /// Gets the parallax factor that determines the scrolling speed of the layer.
        /// </summary>
        internal float ParallaxFactor { get; private set; }

        /// <summary>
        /// Gets the current position of the background layer.
        /// </summary>
        internal Vector2 Position { get; private set; }

        private readonly float initialY;
        private float finalParallaxFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SBackgroundLayer"/> class.
        /// </summary>
        /// <param name="texture">The texture for the background layer.</param>
        /// <param name="textureRectangle">The rectangle defining the portion of the texture to display.</param>
        /// <param name="parallaxFactor">The parallax factor that determines the scrolling speed.</param>
        internal SBackgroundLayer(Texture2D texture, Rectangle textureRectangle, float parallaxFactor)
        {
            this.Texture = texture;
            this.TextureRectangle = textureRectangle;
            this.ParallaxFactor = parallaxFactor;

            this.initialY = (SScreen.Height / 2) - (this.TextureRectangle.Height / 2);
            this.Position = new(SCamera.Center.X / 2, this.initialY);
        }

        /// <summary>
        /// Updates the position of the background layer based on parallax scrolling.
        /// </summary>
        internal void Update()
        {
            this.finalParallaxFactor = this.ParallaxFactor * SBackgroundController.GlobalParallaxFactor;
            this.Position = new(this.Position.X, this.Position.Y + this.finalParallaxFactor);

            if (this.Position.Y > this.initialY * 3)
            {
                this.Position = new(this.Position.X, this.initialY);
            }
        }

        /// <summary>
        /// Draws the background layer on the screen.
        /// </summary>
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this.Texture, this.Position, this.TextureRectangle, Color.White);
            SGraphics.SpriteBatch.Draw(this.Texture, this.Position - new Vector2(0, this.TextureRectangle.Height), this.TextureRectangle, Color.White);
        }
    }
}
