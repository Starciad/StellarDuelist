using Microsoft.Xna.Framework;

using StardustDefender.Core.Enums;

namespace StardustDefender.Core.Projectiles
{
    /// <summary>
    /// Represents a builder for creating projectiles in the game.
    /// </summary>
    public struct SProjectileBuilder
    {
        /// <summary>
        /// Gets or sets the team affiliation of the projectile.
        /// </summary>
        public STeam Team { get; set; }

        /// <summary>
        /// Gets or sets the sprite ID of the projectile.
        /// </summary>
        public int SpriteId { get; set; }

        /// <summary>
        /// Gets or sets the initial position of the projectile.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the initial speed vector of the projectile.
        /// </summary>
        public Vector2 Speed { get; set; }

        /// <summary>
        /// Gets or sets the maximum range the projectile can travel.
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// Gets or sets the damage inflicted by the projectile on impact.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the maximum lifetime of the projectile.
        /// </summary>
        public float LifeTime { get; set; }

        /// <summary>
        /// Gets or sets the color of the projectile.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SProjectileBuilder"/> struct.
        /// </summary>
        public SProjectileBuilder()
        {
            this.Color = Color.White;
        }
    }
}
