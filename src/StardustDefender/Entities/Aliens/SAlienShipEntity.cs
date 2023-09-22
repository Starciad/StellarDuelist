using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;
using StardustDefender.Effects.Common;

using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using StardustDefender.Controllers;
using StardustDefender.Projectiles;

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
        protected override void OnDamaged(int value)
        {
            SSounds.Play("Damage_02");
            SEffectsManager.Create<SImpactEffect>(WorldPosition);

            _ = Task.Run(async () =>
            {
                Color = Color.Red;
                await Task.Delay(235);
                Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            SLevelController.EnemyKilled();

            SSounds.Play("Explosion_01");
            SEffectsManager.Create<SExplosionEffect>(WorldPosition);

            // Drop
            if (SRandom.Chance(25, 100))
                SItemsManager.GetRandomItem(WorldPosition);
        }

        private void MovementUpdate()
        {
            if (currentMovementDelay < movementDelay)
            {
                currentMovementDelay += 0.1f;
            }
            else
            {
                currentMovementDelay = 0;
                switch (movementDirection)
                {
                    case 1:
                        int direction = SRandom.Chance(50, 100) ? -1 : 1;
                        LocalPosition = new(LocalPosition.X + direction, LocalPosition.Y);
                        movementDirection = 2;
                        break;

                    case 2:
                        LocalPosition = new(LocalPosition.X, LocalPosition.Y + 1);
                        movementDirection = 1;
                        break;
                }
            }
        }
        private void ShootUpdate()
        {
            if (currentShootDelay < shootDelay)
            {
                currentShootDelay += 0.1f;
            }
            else
            {
                currentShootDelay = 0;
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

        public override void Reset()
        {
            Animation.Reset();
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
    }
}
