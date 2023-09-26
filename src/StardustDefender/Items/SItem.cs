using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Core;
using StardustDefender.Enums;
using StardustDefender.Managers;

namespace StardustDefender.Items
{
    internal sealed class SItem : IPoolableObject
    {
        internal Vector2 Position { get; set; }

        private const float VERTICAL_SPEED = 1.5f;
        private const float COLLISION_RANGE = 16f;
        private const float COLOR_UPDATE_DELAY = 0.5f;

        private SItemTemplate _template;
        private SAnimation _animation;

        private float currentColorUpdateDelay;
        private int colorIndex;
        private Color color;

        internal void Build(SItemTemplate template, SAnimation animation, Vector2 position)
        {
            this._template = template;
            this._animation = animation;
            this._animation.SetMode(AnimationMode.Disable);

            Position = position;
        }
        internal void Update()
        {
            ColorUpdate();
            MovementUpdate();
            CollisionCheckUpdate();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this._animation.Texture, Position, this._animation.TextureRectangle, this.color, 0f, new Vector2(SItemTemplate.SPRITE_SCALE / 2), 1f, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            _ = SEffectsManager.Create<SImpactEffect>(Position, new(1), 0f, Color.Yellow);
            _ = SSounds.Play("Player_Upgrade");

            SItemsManager.Remove(this);
        }

        private void ColorUpdate()
        {
            if (this.currentColorUpdateDelay < COLOR_UPDATE_DELAY)
            {
                this.currentColorUpdateDelay += 0.1f;
            }
            else
            {
                this.currentColorUpdateDelay = 0;
                this.colorIndex = this.colorIndex < SItemTemplate.COLOR_PALETTE.Length - 1 ? this.colorIndex + 1 : 0;
                this.color = SItemTemplate.COLOR_PALETTE[this.colorIndex];
            }
        }
        private void MovementUpdate()
        {
            float POS_X = Position.X;
            float POS_Y = Position.Y + VERTICAL_SPEED;

            Position = new(POS_X, POS_Y);
        }
        private void CollisionCheckUpdate()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, Position) < COLLISION_RANGE)
            {
                this._template.ApplyEffect();
                Destroy();
            }
        }

        public void Reset()
        {
            Position = Vector2.Zero;
            this.currentColorUpdateDelay = 0f;
            this.colorIndex = 0;
            this.color = Color.White;
        }
    }
}
