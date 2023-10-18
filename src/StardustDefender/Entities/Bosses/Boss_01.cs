using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Animation;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Engine;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Bosses
{
    /// <summary>
    /// [ BLOODY EYE ALIEN ]
    /// </summary>
    /// <remarks>
    /// He moves nimbly left and right, changing his direction by jumping around the corners of the screen, and also moves slightly up and down. At certain time intervals, it stops moving and begins to launch a volley of projectiles that follow random linear directions, when finished, it returns to moving normally.
    /// </remarks>
    [SEntityRegister(typeof(Header))]
    internal sealed class Boss_01 : SBossEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Boss;
            }

            protected override bool OnSpawningCondition()
            {
                SPlayerEntity player = SLevelController.Player;

                return SDifficultyController.DifficultyRate >= 2.5f && SLevelController.Level >= 5 &&
                       player.HealthValue >= 2 && player.HealthValue >= 3.6f && player.BulletLifeTime >= 3.6f;
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

        private const float HORIZONTAL_SPEED = 0.1f;
        private const float VERTICAL_SPEED = 0.01f;

        private const float BULLET_SPEED = 2.5f;
        private const float BULLET_LIFE_TIME = 40f;

        private State state;

        private bool isShooting;
        private bool canMove;
        private bool canShoot;
        private bool horizontalDirection;
        private bool verticalDirection;

        private readonly STimer verticalDirectionTimer = new(10f);
        private readonly STimer shootTimer = new(20f);

        private Vector2 previousLocalPosition;

        // RESET
        public override void Reset()
        {
            // Attributes
            this.HealthValue = 50;
            this.DamageValue = 1;
            this.CollisionRange = 55f;

            // Team
            this.Team = STeam.Bad;

            // States
            this.verticalDirection = false;
            this.horizontalDirection = false;

            // Animation
            this.texture = STextures.GetTexture("ENEMIES_Bosses");
            this.A_Intro.SetTexture(this.texture);
            this.A_Normal.SetTexture(this.texture);
            this.A_Shoot.SetTexture(this.texture);

            this.A_Idle.Reset();
            this.A_Intro.Reset();
            this.A_Normal.Reset();
            this.A_Shoot.Reset();

            this.A_Idle.Clear();
            this.A_Intro.Clear();
            this.A_Normal.Clear();
            this.A_Shoot.Clear();

            this.A_Idle.SetMode(SAnimationMode.Disable);
            this.A_Intro.SetMode(SAnimationMode.Once);
            this.A_Normal.SetMode(SAnimationMode.Disable);
            this.A_Shoot.SetMode(SAnimationMode.Once);

            this.A_Intro.SetDuration(3f);
            this.A_Normal.SetDuration(1f);
            this.A_Shoot.SetDuration(3f);

            this.A_Idle.AddSprite(STextures.GetSprite(64, 0, 0));

            this.A_Intro.AddSprite(STextures.GetSprite(64, 0, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 1, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 2, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 3, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 4, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 5, 0));
            this.A_Intro.AddSprite(STextures.GetSprite(64, 6, 0));

            this.A_Normal.AddSprite(STextures.GetSprite(64, 6, 0));

            this.A_Shoot.AddSprite(STextures.GetSprite(64, 6, 0));
            this.A_Shoot.AddSprite(STextures.GetSprite(64, 7, 0));
            this.A_Shoot.AddSprite(STextures.GetSprite(64, 8, 0));

            this.Animation = this.A_Normal;
        }

        // Override
        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            this.verticalDirectionTimer.Restart();
            this.shootTimer.Restart();

            BOSS_Boost();
            BOSS_Introduction();
        }
        protected override void OnUpdate()
        {
            AnimationUpdate();

            if (this.canMove)
            {
                this.verticalDirectionTimer.Update();
                HorizontalMovementUpdate();
                VerticalMovementUpdate();
                this.previousLocalPosition = this.LocalPosition;
            }

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
            this.HealthValue *= SLevelController.Player.DamageValue;
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
                ? new(this.LocalPosition.X + HORIZONTAL_SPEED, this.LocalPosition.Y)
                : new(this.LocalPosition.X - HORIZONTAL_SPEED, this.LocalPosition.Y);

            if (this.previousLocalPosition.X == this.LocalPosition.X)
            {
                this.horizontalDirection = !this.horizontalDirection;
            }
        }
        private void VerticalMovementUpdate()
        {
            // MOVING
            this.LocalPosition = this.verticalDirection
                ? new(this.LocalPosition.X, this.LocalPosition.Y + VERTICAL_SPEED)
                : new(this.LocalPosition.X, this.LocalPosition.Y - VERTICAL_SPEED);

            // CHANGE DIRECTION
            if (this.verticalDirectionTimer.IsFinished)
            {
                this.verticalDirectionTimer.Restart();
                this.verticalDirection = !this.verticalDirection;
            }
        }
        private void ShootUpdate()
        {
            if (this.isShooting)
            {
                return;
            }

            // Update Delay counters
            if (this.shootTimer.IsFinished)
            {

                _ = Task.Run(StartShootingAsync);
            }
        }

        // Utilities
        private void SetState(State state)
        {
            this.state = state;
        }

        // Actions
        private async Task StartShootingAsync()
        {
            this.A_Shoot.Reset();
            this.A_Shoot.SetMode(SAnimationMode.Once);

            this.state = State.SHOOTING;
            this.isShooting = true;
            this.canMove = false;

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            int shotBurstCount = SRandom.Range(25, 35);
            for (int i = 0; i < shotBurstCount; i++)
            {
                if (this.IsDestroyed)
                {
                    break;
                }

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
                    Damage = this.DamageValue,
                    LifeTime = BULLET_LIFE_TIME,
                    Range = 10f,
                    Color = new Color(255, 0, 0, 255),
                });
                _ = SSounds.Play("Shoot_05");

                await Task.Delay(SRandom.Range(50, 100));
            }

            this.isShooting = false;
            this.canMove = true;
            this.state = State.NORMAL;

            this.shootTimer.Restart();
        }
    }
}
