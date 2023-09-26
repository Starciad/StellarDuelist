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
            Texture = texture;
            TextureRectangle = textureRectangle;
            ParallaxFactor = parallaxFactor;

            this.initialY = (SScreen.Height / 2) - (TextureRectangle.Height / 2);
            Position = new(SCamera.Center.X / 2, this.initialY);
        }

        internal void Update()
        {
            this.finalParallaxFactor = ParallaxFactor * SBackgroundController.GlobalParallaxFactor;
            Position = new(Position.X, Position.Y + this.finalParallaxFactor);

            if (Position.Y > this.initialY * 3)
            {
                Position = new(Position.X, this.initialY);
            }
        }

        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(Texture, Position, TextureRectangle, Color.White);
            SGraphics.SpriteBatch.Draw(Texture, Position - new Vector2(0, TextureRectangle.Height), TextureRectangle, Color.White);
        }
    }
}