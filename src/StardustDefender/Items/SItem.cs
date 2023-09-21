using StardustDefender.Controllers;
using StardustDefender.Animation;
using StardustDefender.Engine;
using StardustDefender.Collections;
using StardustDefender.Enums;
using StardustDefender.Items;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using StardustDefender.Extensions;
using StardustDefender.Managers;
using StardustDefender.Effects.Common;

namespace StardustDefender.Entities.Items
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
            _template = template;
            _animation = animation;
            _animation.SetMode(AnimationMode.Disable);

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
            SGraphics.SpriteBatch.Draw(_animation.Texture, Position, _animation.TextureRectangle, color, 0f, new Vector2(SItemTemplate.SPRITE_SCALE / 2), 1f, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            SEffectsManager.Create<SImpactEffect>(Position, new(1), 0f, Color.Yellow);
            SSounds.Play("Player_Upgrade");

            SItemsManager.Remove(this);
        }

        private void ColorUpdate()
        {
            if (currentColorUpdateDelay < COLOR_UPDATE_DELAY)
            {
                currentColorUpdateDelay += 0.1f;
            }
            else
            {
                currentColorUpdateDelay = 0;
                colorIndex = colorIndex < SItemTemplate.COLOR_PALETTE.Length - 1 ? colorIndex + 1 : 0;
                color = SItemTemplate.COLOR_PALETTE[colorIndex];
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
                _template.ApplyEffect();
                Destroy();
            }
        }

        public void Reset()
        {
            Position = Vector2.Zero;
            currentColorUpdateDelay = 0f;
            colorIndex = 0;
            color = Color.White;
        }
    }
}
