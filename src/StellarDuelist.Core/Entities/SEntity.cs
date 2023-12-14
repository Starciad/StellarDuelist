using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Animation;
using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.World;

using System;

namespace StellarDuelist.Core.Entities
{
    /// <summary>
    /// Represents a reusable instance of an entity.
    /// </summary>
    public abstract class SEntity : IPoolableObject
    {
        #region General
        /// <summary>
        ///
        /// </summary>
        public SEntityDefinition EntityDefinition { get; internal set; }

        /// <summary>
        /// Gets or sets the team to which the entity belongs.
        /// </summary>
        public STeam Team { get; set; }
        #endregion

        #region Texture
        /// <summary>
        /// Gets or sets the animation for the entity.
        /// </summary>
        public SAnimation Animation { get; set; }

        /// <summary>
        /// Gets or sets the color of the entity.
        /// </summary>
        public Color Color { get; set; }
        #endregion

        #region Transform
        /// <summary>
        /// Gets or sets the local position of the entity.
        /// </summary>
        /// <remarks>
        /// The local position of the entity is relative to the division of <see cref="WorldPosition"/> by the current <see cref="SWorld.GridScale"/>.
        /// </remarks>
        public Vector2 LocalPosition
        {
            get => SWorld.GetLocalPosition(this.WorldPosition);
            set => this.WorldPosition = SWorld.GetWorldPosition(value);
        }

        /// <summary>
        /// Gets or sets the global position of the entity.
        /// </summary>
        public Vector2 WorldPosition { get; set; }

        /// <summary>
        /// Gets the current position of the entity.
        /// </summary>
        public Vector2 CurrentPosition { get; private set; }

        /// <summary>
        /// Gets or sets the scale of the entity.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the entity.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the smoothing rate when moving.
        /// </summary>
        public float SmoothScale { get; set; }
        #endregion

        #region Attributes
        /// <summary>
        /// Gets whether the entity is destroyed.
        /// </summary>
        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// Gets or sets the health value of the entity.
        /// </summary>
        public int HealthValue { get; set; }

        /// <summary>
        /// Gets or sets the attack value of the entity.
        /// </summary>
        public int AttackValue { get; set; }
        #endregion

        #region Collision
        /// <summary>
        /// Gets or sets the collision box of the entity.
        /// </summary>
        public Rectangle CollisionBox { get; set; }
        #endregion

        #region Knockback
        /// <summary>
        /// Gets or sets whether the entity can suffer knockback.
        /// </summary>
        public bool CanSufferKnockback { get; set; }

        /// <summary>
        /// Gets or sets the chance of the entity being knocked back.
        /// </summary>
        public int ChanceOfKnockback { get; set; }

        /// <summary>
        /// Gets or sets the force applied to the entity when knocked back.
        /// </summary>
        public int KnockbackForce { get; set; }
        #endregion

        #region Settings
        /// <summary>
        /// Gets or sets whether the entity is invincible.
        /// </summary>
        public bool IsInvincible { get; set; }
        #endregion

        /// <summary>
        /// Initializes the initial components of the entity.
        /// </summary>
        internal void Initialize()
        {
            OnAwake();
            OnStart();

            this.Animation.Initialize();
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        internal void Update()
        {
            this.Animation.Update();

            this.CurrentPosition = Vector2.Lerp(this.CurrentPosition, this.WorldPosition, this.SmoothScale);
            this.LocalPosition = SWorld.ClampHorizontalPosition(this.LocalPosition);

            OnUpdate();
        }

        /// <summary>
        /// Draw the entity.
        /// </summary>
        internal void Draw()
        {
            if (this.Animation.IsEmpty())
            {
                return;
            }

            SGraphics.SpriteBatch.Draw(this.Animation.Texture, this.CurrentPosition, this.Animation.Frame, this.Color, this.Rotation, new Vector2(32 / 2), this.Scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Destroys the current entity.
        /// </summary>
        public void Destroy()
        {
            this.IsDestroyed = true;
            SEntityManager.Remove(this);
            OnDestroy();
        }

        /// <summary>
        /// Deals a certain amount of damage to the entity.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This amount subtracts from the value in <see cref="HealthValue" /> and must be positive.
        ///     </para>
        ///     <para>
        ///         If the entity has <see cref="IsInvincible" /> set to true, this method will have no effect on the entity. Otherwise, the entity will take damage, potentially be knocked back, and may be destroyed.
        ///     </para>
        /// </remarks>
        /// <param name="value">The amount of damage inflicted on the entity.</param>
        public void Damage(int value)
        {
            int temp = Math.Abs(value);

            if (this.IsInvincible)
            {
                return;
            }

            this.HealthValue -= temp;
            OnDamaged(temp);

            if (this.HealthValue <= 0)
            {
                Destroy();
            }
            else
            {
                Knockback();
            }
        }

        /// <summary>
        /// If <see cref="CanSufferKnockback" /> is true, the entity has a chance of being knocked back.
        /// </summary>
        private void Knockback()
        {
            if (!this.CanSufferKnockback)
            {
                return;
            }

            if (!SRandom.Chance(this.ChanceOfKnockback, 100))
            {
                return;
            }

            switch (this.Team)
            {
                case STeam.Good:
                    this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + this.KnockbackForce);
                    break;

                case STeam.Bad:
                    this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y - this.KnockbackForce);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Resets the current entity to default settings.
        /// </summary>
        public virtual void Reset()
        {
            this.LocalPosition = Vector2.Zero;
            this.WorldPosition = Vector2.Zero;
            this.CurrentPosition = Vector2.Zero;
            this.SmoothScale = SWorld.SmoothScale;
            this.Scale = Vector2.One;
            this.Rotation = 0f;
            this.CanSufferKnockback = true;

            this.CollisionBox = new(new((int)this.WorldPosition.X, (int)this.WorldPosition.Y), new(22));
            this.Color = Color.White;

            this.Animation = new();
        }

        /// <summary>
        /// Invoked during the initialization of the entity, just before <see cref="OnStart"/>.
        /// </summary>
        protected virtual void OnAwake() { }

        /// <summary>
        /// Invoked during the initialization of the entity, just after <see cref="OnAwake"/>.
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// Invoked at every fixed frame update.
        /// </summary>
        protected virtual void OnUpdate()
        {
            this.CollisionBox = new(new((int)this.WorldPosition.X, (int)this.WorldPosition.Y), this.CollisionBox.Size);
        }

        /// <summary>
        /// Invoked when the entity is damaged.
        /// </summary>
        /// <param name="value">The total damage value in the context of the call.</param>
        protected virtual void OnDamaged(int value) { }

        /// <summary>
        /// Invoked when the entity is destroyed.
        /// </summary>
        protected virtual void OnDestroy() { }
    }
}
