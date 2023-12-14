using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.Utilities;

using System;

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
    internal sealed partial class Enemy_07 : SEnemyEntity
    {
        #region Definition
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
        #endregion

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
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            this.movementTimer.Start();

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
        protected override void OnStart()
        {
            this.movementTimer.Restart();
            this.shootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();

            // Collision
            if (SEntityCollisionUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI
            MovementUpdate();
            ShootUpdate();
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