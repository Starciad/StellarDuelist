﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Animation;
using StardustDefender.Core.Collections;
using StardustDefender.Core.Colors;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

namespace StardustDefender.Core.Items
{
    /// <summary>
    /// Represents an in-game item.
    /// </summary>
    public sealed class SItem : IPoolableObject
    {
        /// <summary>
        /// Gets or sets the position of the item.
        /// </summary>
        internal Vector2 Position { get; set; }

        private const float VERTICAL_SPEED = 1.5f;
        private const float COLLISION_RANGE = 16f;
        private const float COLOR_UPDATE_DELAY = 0.5f;

        private SItemRegister _register;
        private SAnimation _animation;

        private float currentColorUpdateDelay;
        private int colorIndex;
        private Color color;

        /// <summary>
        /// Builds the item with the provided register, animation, and position.
        /// </summary>
        /// <param name="register">The item's register.</param>
        /// <param name="animation">The item's animation.</param>
        /// <param name="position">The initial position of the item.</param>
        internal void Build(SItemRegister register, SAnimation animation, Vector2 position)
        {
            this._register = register;
            this._animation = animation;
            this._animation.SetMode(SAnimationMode.Disable);

            this.Position = position;
        }

        /// <summary>
        /// Resets the item's properties to their initial state.
        /// </summary>
        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.currentColorUpdateDelay = 0f;
            this.colorIndex = 0;
            this.color = Color.White;
        }

        /// <summary>
        /// Updates the item's properties and behavior.
        /// </summary>
        internal void Update()
        {
            ColorUpdate();
            MovementUpdate();
            CollisionCheckUpdate();
        }

        /// <summary>
        /// Draws the item on the screen.
        /// </summary>
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this._animation.Texture, this.Position, this._animation.Frame, this.color, 0f, new Vector2(SItemRegister.SPRITE_SCALE / 2), 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Destroys the item, triggering any associated effects.
        /// </summary>
        internal void Destroy()
        {
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
                this.colorIndex = this.colorIndex < SPalettes.WARNING_PALETTE.Length - 1 ? this.colorIndex + 1 : 0;
                this.color = SPalettes.WARNING_PALETTE[this.colorIndex];
            }
        }
        private void MovementUpdate()
        {
            float POS_X = this.Position.X;
            float POS_Y = this.Position.Y + VERTICAL_SPEED;

            this.Position = new(POS_X, POS_Y);
        }
        private void CollisionCheckUpdate()
        {
            if (Vector2.Distance(SLevelController.Player.WorldPosition, this.Position) < COLLISION_RANGE)
            {
                this._register.ApplyEffect();
                Destroy();
            }
        }
    }
}
