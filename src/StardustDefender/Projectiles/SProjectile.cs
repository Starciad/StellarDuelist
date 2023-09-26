using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Animation;
using StardustDefender.Collections;
using StardustDefender.Core;
using StardustDefender.Entities;
using StardustDefender.Enums;
using StardustDefender.Managers;

namespace StardustDefender.Projectiles
{
    internal sealed class SProjectile : IPoolableObject
    {
        internal SAnimation Animation { get; private set; } = new();

        public STeam Team { get; private set; }
        public int SpriteId { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; private set; }
        public float Range { get; private set; }
        public int Damage { get; private set; }
        public float LifeTime { get; private set; }
        public Color Color { get; private set; }

        internal void Build(SProjectileBuilder builder)
        {
            Reset();

            this.Team = builder.Team;
            this.SpriteId = builder.SpriteId;
            this.Position = builder.Position;
            this.Speed = builder.Speed;
            this.Range = builder.Range;
            this.Damage = builder.Damage;
            this.LifeTime = builder.LifeTime;
            this.Color = builder.Color;

            this.Animation.AddSprite(STextures.GetSprite(32, this.SpriteId, 0));
            this.Animation.Initialize();
        }

        internal void Initialize()
        {
            this.Animation.SetMode(SAnimationMode.Disable);
            this.Animation.SetTexture(STextures.GetTexture("PROJECTILES_Bullets"));
        }
        internal void Update()
        {
            MovementUpdate();
            LifeTimeUpdate();
            CollisionUpdate();
        }
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this.Animation.Texture, this.Position, this.Animation.TextureRectangle, this.Color, 0f, new Vector2(this.Animation.SpriteScale / 2), 1, SpriteEffects.None, 0f);
        }
        internal void Destroy()
        {
            SProjectileManager.Remove(this);
        }

        public void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Team = STeam.None;
            this.SpriteId = 0;
            this.Position = Vector2.Zero;
            this.Speed = Vector2.Zero;
            this.Range = 0f;
            this.Damage = 0;
            this.LifeTime = 0f;
        }

        private void MovementUpdate()
        {
            this.Position = new(this.Position.X + this.Speed.X, this.Position.Y + this.Speed.Y);
        }
        private void LifeTimeUpdate()
        {
            if (this.LifeTime > 0)
            {
                this.LifeTime -= 0.1f;
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
                    entity?.Team == this.Team ||
                    Vector2.Distance(this.Position, entity.WorldPosition) > entity.CollisionRange)
                {
                    continue;
                }

                entity?.Damage(this.Damage);
                Destroy();
            }
        }
    }
}