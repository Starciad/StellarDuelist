using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Game.Effects;

using System;
using System.Threading.Tasks;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN SPINNER ]
    /// </summary>
    /// <remarks>
    /// It moves strictly downwards after a few seconds. Constantly fires projectiles in a 360 degree radius.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed class Enemy_07 : SEntity
    {
        // ==================================================== //

        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultyRate >= 13;
                });
            }
        }

        // ==================================================== //

        // Bullets
        private const float BULLET_SPEED = 3f;
        private const float BULLET_LIFE_TIME = 50f;

        // Angle
        private const int TOTAL_ANGLES = 20;
        private const float SPREAD_ANGLE_DEGRESS = 360f;
        private const float ANGLE_INCREMENT = SPREAD_ANGLE_DEGRESS / (TOTAL_ANGLES - 1);

        // Properties
        private int currentBullet = TOTAL_ANGLES;
        private float currentAngle = 0f;

        // Timers
        private readonly STimer movementTimer = new(10f);
        private readonly STimer shootTimer = new(1.8f);

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            base.Reset();

            this.movementTimer.Start();

            this.Animation.Reset();
            this.Animation.ClearFrames();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 6));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 6));
            this.Animation.SetDuration(1f);

            this.Team = STeam.Bad;

            this.HealthValue = 45;
            this.AttackValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;

            this.currentBullet = TOTAL_ANGLES;
        }

        // OVERRIDE
        protected override void OnStart()
        {
            this.movementTimer.Restart();
            this.shootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            TimersUpdate();

            // Collision
            if (SEntityUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI
            MovementUpdate();
            ShootUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_08");
            _ = SEffectsManager.Create<ImpactEffect>(this.WorldPosition);

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
            this.movementTimer.Stop();

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(15, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
        }

        // UPDATE
        private void TimersUpdate()
        {
            this.movementTimer.Update();
            this.shootTimer.Update();
        }
        private void MovementUpdate()
        {
            if (!this.movementTimer.IsFinished)
            {
                return;
            }

            this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + 1);
            this.movementTimer.Restart();
        }
        private void ShootUpdate()
        {
            if (!this.shootTimer.IsFinished)
            {
                return;
            }

            if (this.currentBullet > 0)
            {
                Shoot();
                this.currentAngle += ANGLE_INCREMENT;
                this.currentBullet--;
            }
            else
            {
                this.currentBullet = TOTAL_ANGLES;
            }

            this.shootTimer.Restart();
        }

        // SKILLS
        private void Shoot()
        {
            float radians = MathHelper.ToRadians(this.currentAngle);
            Vector2 direction = new((float)Math.Cos(radians), (float)Math.Sin(radians));

            SProjectileManager.Create(new()
            {
                SpriteId = 3,
                Team = STeam.Bad,
                Position = new(this.WorldPosition.X, this.WorldPosition.Y),
                Speed = new(BULLET_SPEED * direction.X, BULLET_SPEED * direction.Y),
                Damage = this.AttackValue,
                LifeTime = BULLET_LIFE_TIME,
                Range = 7
            });

            _ = SSounds.Play("Shoot_04");
        }
    }
}