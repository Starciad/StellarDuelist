using StellarDuelist.Core.Utilities;

namespace StellarDuelist.Core.Entities.Templates
{
    /// <summary>
    /// Base class template for creating player entities.
    /// </summary>
    /// <remarks>
    /// The player is a main entity referenced in all levels. With this template, a variety of functions, properties, and attributes are provided to automate certain processes and have references for internal work/configurations.
    /// </remarks>
    public abstract class SPlayerEntity : SEntity
    {
        /// <summary>
        /// Gets a value indicating whether the player is allowed to shoot.
        /// </summary>
        /// <remarks>
        /// It is <c>true</c> when the <see cref="ShootTimer"/> has finished, otherwise, it's <c>false</c>.
        /// </remarks>
        public bool CanShoot => this.ShootTimer.IsFinished;

        /// <summary>
        /// Gets or sets the durability of the projectiles.
        /// </summary>
        public float BulletLifeTime { get; set; }

        /// <summary>
        /// Gets or sets the speed of the projectiles.
        /// </summary>
        public float BulletSpeed { get; set; }

        /// <summary>
        /// Gets or sets the shooting delay.
        /// </summary>
        public float ShootDelay
        {
            get => this.shootDelay;
            set
            {
                this.shootDelay = value;
                this.ShootTimer.SetDelay(value);
            }
        }

        /// <summary>
        /// Gets the timer used to control shooting delay.
        /// </summary>
        protected STimer ShootTimer { get; private set; } = new();

        private float shootDelay;
    }
}
