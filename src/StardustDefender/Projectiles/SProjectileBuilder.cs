using Microsoft.Xna.Framework;

using StardustDefender.Enums;

namespace StardustDefender.Projectiles
{
    internal struct SProjectileBuilder
    {
        public STeam Team { get; set; }
        public int SpriteId { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public float Range { get; set; }
        public int Damage { get; set; }
        public float LifeTime { get; set; }
        public Color Color { get; set; }

        public SProjectileBuilder()
        {
            this.Color = Color.White;
        }
    }
}
