using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Animation;
using StardustDefender.Core.Collections;
using StardustDefender.Core.Components;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

namespace StardustDefender.Core.Effects
{
    /// <summary>
    /// Reusable instance of an effect created from <see cref="SEffectRegister"/>.
    /// </summary>
    public sealed class SEffect : IPoolableObject
    {
        /// <summary>
        /// Gets or sets the position of the effect.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the scale of the effect.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the effect.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the color of the effect.
        /// </summary>
        public Color Color { get; set; }

        private SAnimation _animation;

        /// <summary>
        /// Resets all components of the effect to their initial state.
        /// </summary>
        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;
        }

        /// <summary>
        /// Builds and prepares the instance of the effect for use.
        /// </summary>
        /// <param name="animation">The animation to be used by the instance.</param>
        internal void Build(SAnimation animation)
        {
            this._animation = animation;
            this._animation.OnAnimationFinished += OnFinished;
            this._animation.SetMode(SAnimationMode.Once);
        }

        /// <summary>
        /// Updates the effect.
        /// </summary>
        internal void Update()
        {
            this._animation.Update();
        }

        /// <summary>
        /// Renders the effect.
        /// </summary>
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
