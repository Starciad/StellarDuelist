using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Core;
using StardustDefender.Enums;
using StardustDefender.Managers;

namespace StardustDefender.Effects
{
    internal sealed class SEffect : IPoolableObject
    {
        internal Vector2 Position { get; set; }
        internal Vector2 Scale { get; set; }
        internal float Rotation { get; set; }
        internal Color Color { get; set; }

        private SAnimation _animation;

        internal void Build(SAnimation animation)
        {
            this._animation = animation;
            this._animation.OnAnimationFinished += OnFinished;
            this._animation.SetMode(AnimationMode.Once);
        }
        internal void Update()
        {
            this._animation.Update();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this._animation.Texture, Position, this._animation.TextureRectangle, Color, Rotation, new Vector2(this._animation.SpriteScale / 2), Scale, SpriteEffects.None, 0f);
        }

        private void OnFinished()
        {
            this._animation.OnAnimationFinished -= OnFinished;
            SEffectsManager.Remove(this);
        }

        public void Reset()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
        }
    }
}
