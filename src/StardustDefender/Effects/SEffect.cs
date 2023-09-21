using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Enums;
using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Engine;
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
            _animation = animation;
            _animation.OnAnimationFinished += OnFinished;
            _animation.SetMode(AnimationMode.Once);
        }
        internal void Update()
        {
            _animation.Update();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(_animation.Texture, Position, _animation.TextureRectangle, Color, Rotation, new Vector2(_animation.SpriteScale / 2), Scale, SpriteEffects.None, 0f);
        }

        private void OnFinished()
        {
            _animation.OnAnimationFinished -= OnFinished;
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
