using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Animation;
using StardustDefender.Core.Collections;
using StardustDefender.Core.Components;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Core.World;

using System;

namespace StardustDefender.Core.Entities
{
    public class SEntity : IPoolableObject
    {
        // General
        public string Id { get; internal set; }
        public STeam Team { get; set; }

        // Texture 
        public SAnimation Animation { get; set; }
        public Color Color { get; set; }

        // Transform
        public Vector2 LocalPosition { get; set; }
        public Vector2 WorldPosition { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        // Attributes
        public bool IsDestroyed { get; private set; }
        public int HealthValue { get; set; }
        public int DamageValue { get; set; }

        // Collision
        public float CollisionRange { get; set; }

        // Knockback
        public int ChanceOfKnockback { get; set; }
        public int KnockbackForce { get; set; }

        // Settings
        public bool IsInvincible { get; set; }

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

        public void Destroy()
        {
            this.IsDestroyed = true;
            SEntityManager.Remove(this);
            OnDestroy();
        }

        public void Damage(int value)
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

        public virtual void Reset() { return; }
        protected virtual void OnAwake() { return; }
        protected virtual void OnStart() { return; }
        protected virtual void OnUpdate() { return; }
        protected virtual void OnDamaged(int value) { return; }
        protected virtual void OnDestroy() { return; }
    }
}
