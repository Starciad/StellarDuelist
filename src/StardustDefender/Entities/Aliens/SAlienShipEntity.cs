using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;

using StardustDefender.Enums;
using StardustDefender.Managers;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Aliens
{
    internal class SAlienShipEntity : SAlien
    {
        private const float SHOOT_SPEED = 2f;
        private const float SHOOT_LIFE_TIME = 25f;

        private readonly float movementDelay = 20f;
        private readonly float shootDelay = 25f;

        private float currentMovementDelay = 0f;
        private float currentShootDelay = 0f;

        private int movementDirection = 1;

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnUpdate()
        {
            // Behaviour
            CollideWithPlayer();

            // AI
            MovementUpdate();
            ShootUpdate();
        }
        public override void Reset()
        {
            Animation.Reset();
            Animation.Clear();

            Animation.SetMode(AnimationMode.Forward);
            Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            Animation.AddSprite(STextures.GetSprite(32, 0, 1));
            Animation.AddSprite(STextures.GetSprite(32, 1, 1));
            Animation.SetDuration(3f);

            Team = Teams.Bad;

            HealthValue = 3;
            DamageValue = 1;

            ChanceOfKnockback = 0;
            KnockbackForce = 0;
        }

        private void MovementUpdate()
        {
            if (this.currentMovementDelay < this.movementDelay)
            {
                this.currentMovementDelay += 0.1f;
            }
            else
            {
                this.currentMovementDelay = 0;
                switch (this.movementDirection)
                {
                    case 1:
                        int direction = SRandom.Chance(50, 100) ? -1 : 1;
                        LocalPosition = new(LocalPosition.X + direction, LocalPosition.Y);
                        this.movementDirection = 2;
                        break;

                    case 2:
                        LocalPosition = new(LocalPosition.X, LocalPosition.Y + 1);
                        this.movementDirection = 1;
                        break;
                }
            }
        }
        private void ShootUpdate()
        {
            if (this.currentShootDelay < this.shootDelay)
            {
                this.currentShootDelay += 0.1f;
            }
            else
            {
                this.currentShootDelay = 0;
                Shoot();
            }
        }

        private void Shoot()
        {
            SProjectileManager.Create(new()
            {
                SpriteId = 0,
                Team = Teams.Bad,
                Position = new(WorldPosition.X, WorldPosition.Y + 16f),
                Speed = new(0, SHOOT_SPEED),
                Damage = DamageValue,
                LifeTime = SHOOT_LIFE_TIME,
                Range = 10f
            });
        }
    }
}
