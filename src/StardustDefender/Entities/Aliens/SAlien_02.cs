using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;

using Microsoft.Xna.Framework;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Aliens
{
    internal sealed class SAlien_02 : SAlienEntity
    {
        private const float SHOOT_SPEED = 2f;
        private const float SHOOT_LIFE_TIME = 25f;

        private readonly STimer movementTimer = new(20f);
        private readonly STimer shootTimer = new(25f);

        private int movementDirection = 1;

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            movementTimer.Restart();
            shootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();

            // Behaviour
            CollideWithPlayer();

            // AI
            MovementUpdate();
            ShootUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_02");
            _ = SEffectsManager.Create<SImpactEffect>(WorldPosition);

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

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<SExplosionEffect>(WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.CreateRandomItem(WorldPosition);
            }
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

        private void TimersUpdate()
        {
            movementTimer.Update();
            shootTimer.Update();
        }

        private void MovementUpdate()
        {
            if (this.movementTimer.IsFinished)
            {
                this.movementTimer.Restart();
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
            if (this.shootTimer.IsFinished)
            {
                this.shootTimer.Restart();
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
