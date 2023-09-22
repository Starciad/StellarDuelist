using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Engine;
using StardustDefender.Entities;
using StardustDefender.Enums;
using StardustDefender.Managers;

namespace StardustDefender.Projectiles
{
    internal sealed class SProjectile : IPoolableObject
    {
        internal SAnimation Animation { get; private set; } = new();

        public Teams Team { get; private set; }
        public int SpriteId { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; private set; }
        public float Range { get; private set; }
        public int Damage { get; private set; }
        public float LifeTime { get; private set; }

        internal void Build(SProjectileBuilder builder)
        {
            Team = builder.Team;
            SpriteId = builder.SpriteId;
            Position = builder.Position;
            Speed = builder.Speed;
            Range = builder.Range;
            Damage = builder.Damage;
            LifeTime = builder.LifeTime;
        }

        internal void Initialize()
        {
            Reset();
        }
        internal void Update()
        {
            MovementUpdate();
            LifeTimeUpdate();
            CollisionUpdate();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(Animation.Texture, Position, Animation.TextureRectangle, Color.White, 0f, new Vector2(Animation.SpriteScale / 2), 1, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            SProjectileManager.Remove(this);
        }

        public void Reset()
        {
            Animation.Reset();
            Animation.SetMode(AnimationMode.Disable);
            Animation.SetTexture(STextures.GetTexture("PROJECTILES_Bullets"));
            Animation.AddSprite(STextures.GetSprite(32, SpriteId, 0));
            Animation.Initialize();

            Team = Teams.None;
            SpriteId = 0;
            Position = Vector2.Zero;
            Speed = Vector2.Zero;
            Range = 0f;
            Damage = 0;
            LifeTime = 0f;
        }

        private void MovementUpdate()
        {
            Position = new(Position.X + Speed.X, Position.Y + Speed.Y);
        }
        private void LifeTimeUpdate()
        {
            if (LifeTime > 0)
            {
                LifeTime -= 0.1f;
            }
            else
            {
                Destroy();
            }
        }
        private void CollisionUpdate()
        {
            foreach (SEntity entity in SEntityManager.Entities)
            {
                if (entity == null ||
                    entity?.Team == Team ||
                    Vector2.Distance(Position, entity.WorldPosition) > Range)
                {
                    continue;
                }

                entity?.Damage(Damage);
                Destroy();
            }
        }
    }
}