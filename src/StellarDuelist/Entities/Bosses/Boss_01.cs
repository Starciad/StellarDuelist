using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Animation;
using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Game.Effects;
using StellarDuelist.Game.Entities.Player;

using System;
using System.Threading.Tasks;

namespace StellarDuelist.Game.Entities.Bosses
{
    /// <summary>
    /// [ BLOODY EYE ALIEN ]
    /// </summary>
    /// <remarks>
    /// He moves nimbly left and right, changing his direction by jumping around the corners of the screen, and also moves slightly up and down. At certain time intervals, it stops moving and begins to launch a volley of projectiles that follow random linear directions, when finished, it returns to moving normally.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed class Boss_01 : SBossEntity
    {
        // ==================================================== //

        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Boss;
                this.canSpawn = new(() =>
                {
                    SPlayerEntity p = SLevelController.Player;

                    return SDifficultyController.DifficultyRate >= 2.5f && SLevelController.Level >= 5 &&
                           p.HealthValue >= 2 && p.HealthValue >= 3.5f && p.BulletLifeTime >= 3.5f;
                });
            }
        }

        // ==================================================== //

        private enum State
        {
            INDLE,
            INTRO,
            NORMAL,
            SHOOTING
        }

        private Texture2D texture;

        private readonly SAnimation A_Idle = new();
        private readonly SAnimation A_Intro = new();
        private readonly SAnimation A_Normal = new();
        private readonly SAnimation A_Shoot = new();

        private float horizontalSpeed = 0.1f;
        private float verticalSpeed = 0.01f;

        private const float BULLET_SPEED = 2.5f;
        private const float BULLET_LIFE_TIME = 40f;

        private const float SHOT_DELAY = 0.05f;

        private State state;

        private bool isShooting;
        private bool canMove;
        private bool canShoot;
        private bool horizontalDirection;
        private bool verticalDirection;

        private readonly STimer verticalDirectionTimer = new(10f);
        private readonly STimer shootTimer = new(20f);

        private int bulletsCount;
        private int shotIndex;
        private float timePassed;

        private Vector2 previousLocalPosition;

        // RESET
        public override void Reset()
        {
            base.Reset();

            // Attributes
            this.HealthValue = 50;
            this.AttackValue = 1;
            this.CollisionBox = new(new((int)this.WorldPosition.X, (int)this.WorldPosition.Y), new(55));

            // Team
            this.Team = STeam.Bad;

            // States
            this.verticalDirection = false;
            this.horizontalDirection = false;

            this.bulletsCount = 20;
            this.shotIndex = 0;
            this.timePassed = 0f;

            // Animation
            this.texture = STextures.GetTexture("ENEMIES_Bosses");
            this.A_Intro.SetTexture(this.texture);
            this.A_Normal.SetTexture(this.texture);
            this.A_Shoot.SetTexture(this.texture);

            this.A_Idle.Reset();
            this.A_Intro.Reset();
            this.A_Normal.Reset();
            this.A_Shoot.Reset();

            this.A_Idle.ClearFrames();
            this.A_Intro.ClearFrames();
            this.A_Normal.ClearFrames();
            this.A_Shoot.ClearFrames();

            this.A_Idle.SetMode(SAnimationMode.Disable);
            this.A_Intro.SetMode(SAnimationMode.Once);
            this.A_Normal.SetMode(SAnimationMode.Disable);
            this.A_Shoot.SetMode(SAnimationMode.Once);

            this.A_Intro.SetDuration(3f);
            this.A_Normal.SetDuration(1f);
            this.A_Shoot.SetDuration(3f);

            this.A_Idle.AddFrame(STextures.GetSprite(64, 0, 0));

            this.A_Intro.AddFrame(STextures.GetSprite(64, 0, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 1, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 2, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 3, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 4, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 5, 0));
            this.A_Intro.AddFrame(STextures.GetSprite(64, 6, 0));

            this.A_Normal.AddFrame(STextures.GetSprite(64, 6, 0));

            this.A_Shoot.AddFrame(STextures.GetSprite(64, 6, 0));
            this.A_Shoot.AddFrame(STextures.GetSprite(64, 7, 0));
            this.A_Shoot.AddFrame(STextures.GetSprite(64, 8, 0));

            this.Animation = this.A_Normal;
        }

        // Override
        protected override void OnStart()
        {
            this.verticalDirectionTimer.Restart();
            this.shootTimer.Restart();

            BOSS_Boost();
            BOSS_Introduction();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            // Collision
            if (SEntityUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
            }

            // Behaviour
            AnimationUpdate();

            // Movement
            if (this.canMove)
            {
                this.verticalDirectionTimer.Update();
                HorizontalMovementUpdate();
                VerticalMovementUpdate();
                this.previousLocalPosition = this.LocalPosition;
            }

            // Shooting
            if (this.canShoot)
            {
                this.shootTimer.Update();
                ShootUpdate();
            }
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_05");
            _ = SEffectsManager.Create<ImpactEffect>(this.WorldPosition, new(2f));

            _ = Task.Run(async () =>
            {
                this.Color = Color.Red;
                await Task.Delay(235);
                this.Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            SLevelController.BossKilled();

            _ = SSounds.Play("Explosion_05");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition, new(2f));

            _ = SItemsManager.CreateRandomItem(new(this.WorldPosition.X, this.WorldPosition.Y + 16));
            _ = SItemsManager.CreateRandomItem(new(this.WorldPosition.X, this.WorldPosition.Y - 16));
            _ = SItemsManager.CreateRandomItem(new(this.WorldPosition.X + 16, this.WorldPosition.Y));
            _ = SItemsManager.CreateRandomItem(new(this.WorldPosition.X - 16, this.WorldPosition.Y));
        }

        // Actions
        private void BOSS_Boost()
        {
            SPlayerEntity p = SLevelController.Player;

            // Boost health and bullets
            this.HealthValue *= p.AttackValue;
            this.bulletsCount += (int)Math.Round(p.HealthValue + (p.AttackValue / 2f));

            // Boost movement
            if (p.ShootDelay <= 1.5f)
            {
                this.horizontalSpeed = 0.2f;
                this.verticalSpeed = 0.02f;
            }
        }
        private void BOSS_Introduction()
        {
            _ = Task.Run(async () =>
            {
                this.IsInvincible = true;
                this.canMove = false;
                this.canShoot = false;

                SetState(State.INDLE);
                await Task.Delay(TimeSpan.FromSeconds(1f));
                SetState(State.INTRO);
                await Task.Delay(TimeSpan.FromSeconds(3.5f));
                SetState(State.NORMAL);

                this.IsInvincible = false;
                this.canMove = true;
                this.canShoot = true;
            });
        }

        // Update
        private void AnimationUpdate()
        {
            this.Animation = this.state switch
            {
                State.INTRO => this.A_Intro,
                State.INDLE => this.A_Idle,
                State.NORMAL => this.A_Normal,
                State.SHOOTING => this.A_Shoot,
                _ => this.A_Idle,
            };

            this.Animation?.Update();
        }
        private void HorizontalMovementUpdate()
        {
            // MOVING
            this.LocalPosition = this.horizontalDirection
                ? new(this.LocalPosition.X + this.horizontalSpeed, this.LocalPosition.Y)
                : new(this.LocalPosition.X - this.horizontalSpeed, this.LocalPosition.Y);

            if (this.previousLocalPosition.X == this.LocalPosition.X)
            {
                this.horizontalDirection = !this.horizontalDirection;
            }
        }
        private void VerticalMovementUpdate()
        {
            // MOVING
            this.LocalPosition = this.verticalDirection
                ? new(this.LocalPosition.X, this.LocalPosition.Y + this.verticalSpeed)
                : new(this.LocalPosition.X, this.LocalPosition.Y - this.verticalSpeed);

            // CHANGE DIRECTION
            if (this.verticalDirectionTimer.IsFinished)
            {
                this.verticalDirectionTimer.Restart();
                this.verticalDirection = !this.verticalDirection;
            }
        }
        private void ShootUpdate()
        {
            if (this.shootTimer.IsFinished)
            {
                if (!this.isShooting)
                { StartShooting(); }

                if (this.isShooting)
                { ShootingUpdate(); }
            }
        }

        // Utilities
        private void SetState(State state)
        {
            this.state = state;
        }

        // Actions
        private void StartShooting()
        {
            this.A_Shoot.Reset();
            this.A_Shoot.SetMode(SAnimationMode.Once);

            this.state = State.SHOOTING;
            this.isShooting = true;
            this.canMove = false;
        }
        private void ShootingUpdate()
        {
            this.timePassed += 0.01f;

            if (this.timePassed >= SHOT_DELAY)
            {
                this.timePassed -= SHOT_DELAY;

                Vector2 bulletSpeed = new(
                    BULLET_SPEED * (SRandom.Range(-1, 2) + (-SRandom.NextFloat() / 1.5f)),
                    BULLET_SPEED
                );

                SProjectileManager.Create(new()
                {
                    SpriteId = 1,
                    Team = STeam.Bad,
                    Position = new(this.WorldPosition.X + 16, this.WorldPosition.Y + 16),
                    Speed = bulletSpeed,
                    Damage = this.AttackValue,
                    LifeTime = BULLET_LIFE_TIME,
                    Range = 10,
                    Color = new Color(255, 0, 0, 255),
                });
                _ = SSounds.Play("Shoot_05");

                this.shotIndex++;

                if (this.shotIndex >= this.bulletsCount)
                {
                    this.isShooting = false;
                    this.canMove = true;
                    this.state = State.NORMAL;
                    this.shootTimer.Restart();

                    this.timePassed = 0f;
                    this.shotIndex = 0;
                    return;
                }
            }
        }
    }
}
