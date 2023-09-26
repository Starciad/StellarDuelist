using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Core;

namespace StardustDefender.Background.Layers
{
    internal sealed class SBackgroundLayer
    {
        internal Texture2D Texture { get; private set; }
        internal Rectangle TextureRectangle { get; private set; }
        internal float ParallaxFactor { get; private set; }
        internal Vector2 Position { get; private set; }

        private readonly float initialY;
        private float finalParallaxFactor;

        internal SBackgroundLayer(Texture2D texture, Rectangle textureRectangle, float parallaxFactor)
        {
            this.Texture = texture;
            this.TextureRectangle = textureRectangle;
            this.ParallaxFactor = parallaxFactor;

            this.initialY = (SScreen.Height / 2) - (this.TextureRectangle.Height / 2);
            this.Position = new(SCamera.Center.X / 2, this.initialY);
        }

        internal void Update()
        {
            this.finalParallaxFactor = this.ParallaxFactor * SBackgroundController.GlobalParallaxFactor;
            this.Position = new(this.Position.X, this.Position.Y + this.finalParallaxFactor);

            if (this.Position.Y > this.initialY * 3)
            {
                this.Position = new(this.Position.X, this.initialY);
            }
        }

        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this.Texture, this.Position, this.TextureRectangle, Color.White);
            SGraphics.SpriteBatch.Draw(this.Texture, this.Position - new Vector2(0, this.TextureRectangle.Height), this.TextureRectangle, Color.White);
        }
    }
}