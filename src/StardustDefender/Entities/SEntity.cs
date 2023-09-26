using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Core;
using StardustDefender.Enums;
using StardustDefender.Managers;
using StardustDefender.World;

using System;

namespace StardustDefender.Entities
{
    internal class SEntity : IPoolableObject
    {
        // General
        internal string Id { get; set; }
        internal STeam Team { get; set; }

        // Texture 
        internal SAnimation Animation { get; set; }
        internal Color Color { get; set; }

        // Transform
        internal Vector2 LocalPosition { get; set; }
        internal Vector2 WorldPosition { get; set; }
        internal Vector2 Scale { get; set; }
        internal float Rotation { get; set; }

        // Attributes
        internal int HealthValue { get; set; }
        internal int DamageValue { get; set; }

        // Collision
        internal float CollisionRange { get; set; }

        // Knockback
        internal int ChanceOfKnockback { get; set; }
        internal int KnockbackForce { get; set; }

        // Settings
        internal bool IsInvincible { get; set; }

        public SEntity()
        {
            this.Id = Guid.NewGuid().ToString();

            this.LocalPosition = Vector2.Zero;
            this.WorldPosition = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;

            this.CollisionRange = 22f;
            this.Color = Color.White;
        }

        internal void Initialize()
        {
            this.Animation = new();

            OnAwake();
            OnStart();

            this.Animation.Initialize();
        }
        internal void Update()
        {
            this.Animation.Update();

            this.WorldPosition = Vector2.Lerp(this.WorldPosition, SWorld.GetWorldPosition(this.LocalPosition), SWorld.SmoothScale);
            this.LocalPosition = SWorld.Clamp(this.LocalPosition);

            OnUpdate();
        }
        internal void Draw()
        {
            if (this.Animation.IsEmpty())
            {
                return;
            }

            SGraphics.SpriteBatch.Draw(this.Animation.Texture, this.WorldPosition, this.Animation.TextureRectangle, this.Color, this.Rotation, new Vector2(32 / 2), this.Scale, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            SEntityManager.Remove(this);
            OnDestroy();
        }

        public virtual void Reset() { return; }

        internal void Damage(int value)
        {
            if (this.IsInvincible)
            {
                return;
            }

            this.HealthValue -= value;
            OnDamaged(value);

            if (this.HealthValue <= 0)
            {
                Destroy();
            }
            else
            {
                Knockback();
            }
        }
        private void Knockback()
        {
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

        protected virtual void OnAwake() { return; }
        protected virtual void OnStart() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDamaged(int value) { return; }
        protected virtual void OnDestroy() { return; }
    }
}
