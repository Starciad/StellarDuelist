using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Animation;
using StardustDefender.Core.Collections;
using StardustDefender.Core.Components;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

namespace StardustDefender.Core.Effects
{
    public sealed class SEffect : IPoolableObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }

        private SAnimation _animation;

        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;
        }

        internal void Build(SAnimation animation)
        {
            this._animation = animation;
            this._animation.OnAnimationFinished += OnFinished;
            this._animation.SetMode(SAnimationMode.Once);
        }
        internal void Update()
        {
            this._animation.Update();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this._animation.Texture, this.Position, this._animation.Frame, this.Color, this.Rotation, new Vector2(this._animation.SpriteScale / 2), this.Scale, SpriteEffects.None, 0f);
        }

        private void OnFinished()
        {
            this._animation.OnAnimationFinished -= OnFinished;
            SEffectsManager.Remove(this);
        }
    }
}
