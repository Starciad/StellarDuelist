using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Engine;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN EYE ]
    /// </summary>
    /// <remarks>
    /// Moves quickly down, left, and right, with a very high, random jump radius. When he finishes moving, he shoots 3 projectiles in a linear direction at the player.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Header))]
    internal sealed class Enemy_05 : SEnemyEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Enemy;
            }

            protected override bool OnSpawningCondition()
            {
                return SDifficultyController.DifficultyRate >= 9;
            }
        }

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
        // RESET
        public override void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 4));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 4));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 25;
            this.DamageValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        // OVERRIDE
        protected override void OnAwake()
        {
            Reset();
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

            // Behaviour
            CollideWithPlayer();

            // AI (Move + Shoot)
            ShootingUpdate();
            MovementUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_05");
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

            _ = SSounds.Play("Explosion_04");
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
                return;

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
                Damage = this.DamageValue,
                LifeTime = BULLET_LIFE_TIME,
                Range = 10f,
                Color = Color.White,
            });

            _ = SSounds.Play("Shoot_06");
        }
    }
}