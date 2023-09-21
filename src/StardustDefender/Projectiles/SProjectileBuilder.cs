using StardustDefender.Enums;

using Microsoft.Xna.Framework;

namespace StardustDefender.Projectiles
{
    internal struct SProjectileBuilder
    {
        public Teams Team { get; set; }
        public int SpriteId { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public float Range { get; set; }
        public int Damage { get; set; }
        public float LifeTime { get; set; }
    }
}
