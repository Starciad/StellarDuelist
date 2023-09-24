using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Entities.Player;
using StardustDefender.Managers;
using StardustDefender.Enums;
using StardustDefender.Animation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Threading.Tasks;
using System;

namespace StardustDefender.Entities.Bosses
{
    internal sealed class SBoss_01 : SBossEntity
    {
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
        private const float VERTICAL_SPEED = 0.1f;

        private const float BULLET_SPEED = 1.5f;
        private const float BULLET_LIFE_TIME = 40f;
        
        private const float DELAY_TO_CHANGE_VERTICAL_DIRECTION = 5f;
        private const float DELAY_FOR_SHOOTING = 20f;

        private State state;

        private bool isDied;
        private bool isShooting;
        private bool canMove;
        private bool canShoot;
        private bool horizontalDirection;
        private bool verticalDirection;

        private float currentDelayToChangeVerticalDirection;
        private float currentDelayForShooting;

        private Vector2 previousLocalPosition;

        // Override
        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            BOSS_Introduction();
        }
        protected override void OnUpdate()
        {
            // Animation
            AnimationUpdate();

            // IA
            if (canMove)
            {
                HorizontalMovementUpdate();
                VerticalMovementUpdate();
                previousLocalPosition = LocalPosition;
            }

            if (canShoot)
            {
                ShootUpdate();
            }
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_05");
            _ = SEffectsManager.Create<SImpactEffect>(WorldPosition, new(2f));

            _ = Task.Run(async () =>
            {
                Color = Color.Red;
                await Task.Delay(235);
                Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            isDied = true;
            SLevelController.BossKilled();

            _ = SSounds.Play("Explosion_05");
            _ = SEffectsManager.Create<SExplosionEffect>(WorldPosition, new(2f));

            _ = SItemsManager.CreateRandomItem(new(WorldPosition.X, WorldPosition.Y + 16));
            _ = SItemsManager.CreateRandomItem(new(WorldPosition.X, WorldPosition.Y - 16));
            _ = SItemsManager.CreateRandomItem(new(WorldPosition.X + 16, WorldPosition.Y));
            _ = SItemsManager.CreateRandomItem(new(WorldPosition.X - 16, WorldPosition.Y));
        }
        public override void Reset()
        {
            // Attributes
            HealthValue = 20;
            DamageValue = 1;
            CollisionRange = 55f;

            // Team
            Team = Teams.Bad;

            // States
            isDied = false;

            // Counters
            currentDelayToChangeVerticalDirection = 0;
            currentDelayForShooting = 0;

            // Animation
            texture = STextures.GetTexture("ENEMIES_Bosses");
            A_Intro.SetTexture(texture);
            A_Normal.SetTexture(texture);
            A_Shoot.SetTexture(texture);

            A_Idle.Reset();
            A_Intro.Reset();
            A_Normal.Reset();
            A_Shoot.Reset();

            A_Idle.Clear();
            A_Intro.Clear();
            A_Normal.Clear();
            A_Shoot.Clear();

            A_Idle.SetMode(AnimationMode.Disable);
            A_Intro.SetMode(AnimationMode.Once);
            A_Normal.SetMode(AnimationMode.Disable);
            A_Shoot.SetMode(AnimationMode.Once);

            A_Intro.SetDuration(3f);
            A_Normal.SetDuration(1f);
            A_Shoot.SetDuration(3f);

            A_Idle.AddSprite(STextures.GetSprite(64, 0, 0));

            A_Intro.AddSprite(STextures.GetSprite(64, 0, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 1, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 2, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 3, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 4, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 5, 0));
            A_Intro.AddSprite(STextures.GetSprite(64, 6, 0));

            A_Normal.AddSprite(STextures.GetSprite(64, 6, 0));

            A_Shoot.AddSprite(STextures.GetSprite(64, 6, 0));
            A_Shoot.AddSprite(STextures.GetSprite(64, 7, 0));
            A_Shoot.AddSprite(STextures.GetSprite(64, 8, 0));

            Animation = A_Normal;

            // Start
            OnStart();
        }

        // Animations
        private void BOSS_Introduction()
        {
            _ = Task.Run(async () =>
            {
                IsInvincible = true;
                canMove = false;
                canShoot = false;

                SetState(State.INDLE);
                await Task.Delay(TimeSpan.FromSeconds(1f));
                SetState(State.INTRO);
                await Task.Delay(TimeSpan.FromSeconds(3.5f));
                SetState(State.NORMAL);

                IsInvincible = false;
                canMove = true;
                canShoot = true;
            });
        }

        // Update
        private void AnimationUpdate()
        {
            Animation = state switch
            {
                State.INTRO => A_Intro,
                State.INDLE => A_Idle,
                State.NORMAL => A_Normal,
                State.SHOOTING => A_Shoot,
                _ => A_Idle,
            };

            Animation?.Update();
        }
        private void HorizontalMovementUpdate()
        {
            // MOVING
            if (horizontalDirection)
            {
                LocalPosition = new(LocalPosition.X + HORIZONTAL_SPEED, LocalPosition.Y);
            }
            else
            {
                LocalPosition = new(LocalPosition.X - HORIZONTAL_SPEED, LocalPosition.Y);
            }

            if (previousLocalPosition.X == LocalPosition.X)
            {
                horizontalDirection = !horizontalDirection;
            }
        }
        private void VerticalMovementUpdate()
        {
            // MOVING
            if (verticalDirection)
            {
                LocalPosition = Vector2.Lerp(LocalPosition, new(LocalPosition.X, LocalPosition.Y + VERTICAL_SPEED), 0.1f);
            }
            else
            {
                LocalPosition = Vector2.Lerp(LocalPosition, new(LocalPosition.X, LocalPosition.Y - VERTICAL_SPEED), 0.1f);
            }

            // CHANGE DIRECTION
            if (currentDelayToChangeVerticalDirection < DELAY_TO_CHANGE_VERTICAL_DIRECTION)
            {
                currentDelayToChangeVerticalDirection += 0.1f;
            }
            else
            {
                currentDelayToChangeVerticalDirection = 0f;
                verticalDirection = !verticalDirection;
            }
        }
        private void ShootUpdate()
        {
            if (isShooting)
                return;

            // Update Delay counters
            if (currentDelayForShooting < DELAY_FOR_SHOOTING)
            {
                currentDelayForShooting += 0.1f;
            }
            else
            {
                currentDelayForShooting = 0f;
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
            A_Shoot.Reset();
            A_Shoot.SetMode(AnimationMode.Once);

            this.state = State.SHOOTING;

            isShooting = true;
            canMove = false;

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            int shotBurstCount = SRandom.Range(5, 15);
            for (int i = 0; i < shotBurstCount; i++)
            {
                if (isDied)
                {
                    break;
                }

                Vector2 bulletSpeed = new(
                    BULLET_SPEED * (SRandom.Range(-1, 2) + -SRandom.NextFloat() / 1.5f),
                    BULLET_SPEED
                );

                SProjectileManager.Create(new()
                {
                    SpriteId = 1,
                    Team = Teams.Bad,
                    Position = new(WorldPosition.X + 16, WorldPosition.Y + 16),
                    Speed = bulletSpeed,
                    Damage = DamageValue,
                    LifeTime = BULLET_LIFE_TIME,
                    Range = 10f,
                    Color = new Color(255, 0, 0, 255),
                });

                await Task.Delay(TimeSpan.FromSeconds(SRandom.Range(0, 1)));
            }

            isShooting = false;
            canMove = true;

            this.state = State.NORMAL;
        }
    }
}
