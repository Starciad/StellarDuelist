using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Animation;
using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;

namespace StellarDuelist.Core.Projectiles
{
    /// <summary>
    /// Represents a projectile in the game.
    /// </summary>
    internal sealed class SProjectile : IPoolableObject
    {
        /// <summary>
        /// Gets the animation associated with the projectile.
        /// </summary>
        internal SAnimation Animation { get; private set; } = new();

        /// <summary>
        /// Gets the team affiliation of the projectile.
        /// </summary>
        public STeam Team { get; private set; }

        /// <summary>
        /// Gets the sprite ID of the projectile.
        /// </summary>
        public int SpriteId { get; private set; }

        /// <summary>
        /// Gets the position of the projectile.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Gets the speed vector of the projectile.
        /// </summary>
        public Vector2 Speed { get; private set; }

        /// <summary>
        /// Gets the maximum range the projectile can travel.
        /// </summary>
        public int Range { get; private set; }

        /// <summary>
        /// Gets the damage inflicted by the projectile on impact.
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// Gets the remaining lifetime of the projectile.
        /// </summary>
        public float LifeTime { get; private set; }

        /// <summary>
        /// Gets the color of the projectile.
        /// </summary>
        public Color Color { get; private set; }

        private Rectangle collisionBox;

        /// <summary>
        /// Builds the projectile using the specified builder.
        /// </summary>
        /// <param name="builder">The builder containing projectile information.</param>
        internal void Build(SProjectileBuilder builder)
        {
            this.Team = builder.Team;
            this.SpriteId = builder.SpriteId;
            this.Position = builder.Position;
            this.Speed = builder.Speed;
            this.Range = builder.Range;
            this.Damage = builder.Damage;
            this.LifeTime = builder.LifeTime;
            this.Color = builder.Color;
            this.collisionBox = new(new((int)this.Position.X, (int)this.Position.Y), new(this.Range));

            this.Animation.AddFrame(STextures.GetSprite(32, this.SpriteId, 0));
            this.Animation.Initialize();
        }

        /// <summary>
        /// Initializes the projectile.
        /// </summary>
        internal void Initialize()
        {
            this.Animation.SetMode(SAnimationMode.Disable);
            this.Animation.SetTexture(STextures.GetTexture("PROJECTILES_Bullets"));
        }

        /// <summary>
        /// Updates the projectile's state and behavior.
        /// </summary>
        internal void Update()
        {
            CollisionUpdate();
            LifeTimeUpdate();
            MovementUpdate();
        }

        /// <summary>
        /// Draws the projectile on the screen.
        /// </summary>
        internal void Draw()
        {
            SGraphics.SpriteBatch.Draw(this.Animation.Texture, this.Position, this.Animation.Frame, this.Color, 0f, new Vector2(this.Animation.SpriteScale / 2), 1, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Destroys the projectile, removing it from the game.
        /// </summary>
        internal void Destroy()
        {
            SProjectileManager.Remove(this);
        }

        /// <summary>
        /// Resets the projectile's properties to their default values.
        /// </summary>
        public void Reset()
        {
            this.Animation.Reset();
            this.Animation.ClearFrames();

            this.Team = STeam.None;
            this.SpriteId = 0;
            this.Position = Vector2.Zero;
            this.Speed = Vector2.Zero;
            this.Range = 0;
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
            this.collisionBox = new(new((int)this.Position.X, (int)this.Position.Y), this.collisionBox.Size);

            foreach (SEntity entity in SEntityManager.ActiveEntities)
            {
                if (entity == null || entity.Team == this.Team || !this.collisionBox.Intersects(entity.CollisionBox))
                {
                    continue;
                }

                entity.Damage(this.Damage);
                Destroy();
            }
        }
    }
}
