using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;

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
            this.movementTimer.Restart();
            this.shootTimer.Restart();
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
            _ = SEffectsManager.Create<SImpactEffect>(this.WorldPosition);

            _ = Task.Run(async () =>
            {
                this.Color = Color.Red;
                await Task.Delay(235);
                this.Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            SLevelController.EnemyKilled();

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<SExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
        }
        public override void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(AnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 1));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 1));
            this.Animation.SetDuration(3f);

            this.Team = Teams.Bad;

            this.HealthValue = 3;
            this.DamageValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        private void TimersUpdate()
        {
            this.movementTimer.Update();
            this.shootTimer.Update();
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
                        this.LocalPosition = new(this.LocalPosition.X + direction, this.LocalPosition.Y);
                        this.movementDirection = 2;
                        break;

                    case 2:
                        this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + 1);
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
                Position = new(this.WorldPosition.X, this.WorldPosition.Y + 16f),
                Speed = new(0, SHOOT_SPEED),
                Damage = this.DamageValue,
                LifeTime = SHOOT_LIFE_TIME,
                Range = 10f
            });
        }
    }
}
