using StardustDefender.Core.Engine;

namespace StardustDefender.Core.Entities.Templates
{
    public abstract class SPlayerEntity : SEntity
    {
        public bool CanShoot => this.ShootTimer.IsFinished;
        public float BulletLifeTime { get; set; }
        public float BulletSpeed { get; set; }
        public float ShootDelay
        {
            get => this.shootDelay;

            set
            {
                this.shootDelay = value;
                this.ShootTimer.SetDelay(value);
            }
        }
        protected STimer ShootTimer { get; private set; } = new();

        private float shootDelay;
    }
}