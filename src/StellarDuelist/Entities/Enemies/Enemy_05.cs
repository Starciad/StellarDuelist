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

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN EYE ]
    /// </summary>
    /// <remarks>
    /// Moves quickly down, left, and right, with a very high, random jump radius. When he finishes moving, he shoots 3 projectiles in a linear direction at the player.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed partial class Enemy_05 : SEnemyEntity
    {
        #region Definition
        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultyRate >= 9;
                });
            }
        }
        #endregion

        // ==================================================== //

        private const float BULLET_SPEED = 2.5f;
        private const float BULLET_LIFE_TIME = 30f;
        private const int MAX_BULLETS = 3;

        private readonly STimer shootTimer = new(10f);
        private readonly STimer intervalBetweenShots = new(1f);
        private readonly STimer movementTimer = new(20f);

        private SPlayerEntity player;

        private int currentBullet;
        private bool canShoot;

        // ==================================================== //
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 4));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 4));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 25;
            this.AttackValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }
        protected override void OnStart()
        {
            this.movementTimer.Restart();
            this.shootTimer.Restart();
            this.intervalBetweenShots.Start();

            this.player = SLevelController.Player;
        }
        protected override void OnUpdate()
        {
            // Timers
            TimersUpdate();

            // Collision
            if (SEntityCollisionUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI (Move + Shoot)
            ShootingUpdate();
            MovementUpdate();
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

            this.movementTimer.Restart();
            this.LocalPosition = new(this.LocalPosition.X + SRandom.Range(-3, 4), this.LocalPosition.Y + SRandom.Range(1, 3));

            this.canShoot = true;
        }
        private void ShootingUpdate()
        {
            if (!this.canShoot)
            {
                return;
            }

            if (!this.shootTimer.IsFinished)
            {
                return;
            }

            if (this.currentBullet < MAX_BULLETS)
            {
                Shoot();
            }
            else
            {
                this.canShoot = false;
                this.currentBullet = 0;
                this.intervalBetweenShots.Restart();
                this.shootTimer.Restart();
            }
        }

        // SKILLS
        private void Shoot()
        {
            // ========================= //
            // Delay

            this.intervalBetweenShots.Update();
            if (!this.intervalBetweenShots.IsFinished)
            {
                return;
            }

            this.intervalBetweenShots.Restart();
            this.currentBullet++;

            // ========================= //

            Vector2 direction = this.player.WorldPosition - this.WorldPosition;

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            direction *= BULLET_SPEED;

            SProjectileManager.Create(new()
            {
                SpriteId = 1,
                Team = STeam.Bad,
                Position = new(this.WorldPosition.X, this.WorldPosition.Y),
                Speed = direction,
                Damage = this.AttackValue,
                LifeTime = BULLET_LIFE_TIME,
                Size = new(10, 10),
                Color = Color.White,
            });

            _ = SSounds.Play("Shoot_06");
        }
    }
}