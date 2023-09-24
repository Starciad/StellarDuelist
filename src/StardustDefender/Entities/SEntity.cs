using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Engine;

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
        internal Teams Team { get; set; }

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
            Id = Guid.NewGuid().ToString();

            LocalPosition = Vector2.Zero;
            WorldPosition = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;

            CollisionRange = 22f;
            Color = Color.White;
        }

        internal void Initialize()
        {
            Animation = new();

            OnAwake();
            OnStart();

            Animation.Initialize();
        }
        internal void Update()
        {
            Animation.Update();

            WorldPosition = Vector2.Lerp(WorldPosition, SWorld.GetWorldPosition(LocalPosition), SWorld.SmoothScale);
            LocalPosition = SWorld.Clamp(LocalPosition);

            OnUpdate();
        }
        internal void Draw()
        {
            if (Animation.IsEmpty())
                return;

            SGraphics.SpriteBatch.Draw(Animation.Texture, WorldPosition, Animation.TextureRectangle, Color, Rotation, new Vector2(32 / 2), Scale, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            SEntityManager.Remove(this);
            OnDestroy();
        }

        public virtual void Reset() { return; }

        internal void Damage(int value)
        {
            if (IsInvincible)
                return;

            HealthValue -= value;
            OnDamaged(value);

            if (HealthValue <= 0)
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
            if (!SRandom.Chance(ChanceOfKnockback, 100))
            {
                return;
            }

            switch (Team)
            {
                case Teams.Good:
                    LocalPosition = new(LocalPosition.X, LocalPosition.Y + KnockbackForce);
                    break;

                case Teams.Bad:
                    LocalPosition = new(LocalPosition.X, LocalPosition.Y - KnockbackForce);
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
