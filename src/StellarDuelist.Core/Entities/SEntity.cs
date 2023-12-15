using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Animation;
using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Collision;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.SEventArgs.Entities;
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
        /// Gets the definition information for the current entity.
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
        /// Gets or sets the collision of the entity.
        /// </summary>
        public SCollision Collision { get; private set; } = new();
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

        #region Consts
        public const int DEFAULT_ENTITY_SIZE = 22;
        public const int DEFAULT_BOSS_SIZE = 55;
        #endregion

        #region Events
        public delegate void DamagedEventHandler(SEntityDamagedEventArgs e);
        public delegate void HealedEventHandler(SEntityHealedEventArgs e);
        public delegate void DestroyedEventHandler();

        public event DamagedEventHandler OnDamaged;
        public event HealedEventHandler OnHealed;
        public event DestroyedEventHandler OnDestroyed;
        #endregion

        #region System
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
            this.Color = Color.White;

            this.Collision.SetSize(new(DEFAULT_ENTITY_SIZE));

            this.Animation ??= new();
            this.Animation?.ClearFrames();
            this.Animation?.Reset();

            OnUnsubscribeEvents();
            OnSubscribeEvents();
        }

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
            UpdateEntityAnimation();
            UpdateEntityPosition();
            UpdateEntityCollision();
            UpdateHealthCheck();
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

            SGraphics.SpriteBatch.Draw(this.Animation.Texture, this.CurrentPosition, this.Animation.Frame, this.Color, this.Rotation, new Vector2(this.Animation.SpriteScale / 2), this.Scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Destroys the current entity.
        /// </summary>
        public void Destroy()
        {
            this.IsDestroyed = true;
            SEntityManager.Remove(this);
            OnDestroyed?.Invoke();
        }
        #endregion

        #region Updates
        private void UpdateEntityAnimation()
        {
            this.Animation.Update();
        }
        private void UpdateEntityPosition()
        {
            this.CurrentPosition = Vector2.Lerp(this.CurrentPosition, this.WorldPosition, this.SmoothScale);
            this.LocalPosition = SWorld.ClampHorizontalPosition(this.LocalPosition);
        }
        private void UpdateEntityCollision()
        {
            this.Collision.SetPosition(this.CurrentPosition.ToPoint());
        }
        private void UpdateHealthCheck()
        {
            if (!this.IsInvincible && this.HealthValue <= 0)
            {
                Destroy();
            }
        }
        #endregion

        #region Utilities
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
            if (this.IsInvincible)
            {
                return;
            }

            int damageValue = Math.Abs(value);
            this.HealthValue -= damageValue;
            OnDamaged?.Invoke(new(damageValue));

            if (this.HealthValue > 0)
            {
                Knockback();
            }
        }

        /// <summary>
        ///Adds health to the current entity.
        /// </summary>
        /// <param name="value">The health value that will be added to the current entity.</param>
        public void Heal(int value)
        {
            this.HealthValue += value;
            OnHealed?.Invoke(new(value));
        }

        /// <summary>
        /// If <see cref="CanSufferKnockback" /> is true, the entity has a chance of being knocked back.
        /// </summary>
        public void Knockback()
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
        #endregion

        #region Events (Methods)
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
        protected virtual void OnUpdate() { }

        /// <summary>
        ///Invoked during entity initialization.
        /// </summary>
        /// <remarks>
        /// Space reserved for the registration of inventions.
        /// </remarks>
        protected virtual void OnSubscribeEvents() { }

        /// <summary>
        /// Invoked during entity initialization.
        /// </summary>
        /// <remarks>
        /// Space reserved for unregistering inventions.
        /// </remarks>
        protected virtual void OnUnsubscribeEvents() { }
        #endregion
    }
}
