using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Player
{
    internal sealed class SPlayerEntity : SEntity
    {
        internal bool CanShoot => this.currentShootDelay == 0;
        public float ShootDelay { get; set; } = 3f;
        public float BulletLifeTime { get; set; } = 3f;
        public float BulletSpeed { get; set; } = 3f;

        private float currentShootDelay;

        protected override void OnAwake()
        {
            Animation.SetTexture(STextures.GetTexture("PLAYER_Spaceship"));
            Animation.AddSprite(STextures.GetSprite(32, 0, 0));

            Team = Teams.Good;

            HealthValue = 3;
            DamageValue = 1;

            ChanceOfKnockback = 0;
            KnockbackForce = 0;
        }
        protected override void OnUpdate()
        {
            ShootUpdate();
            InputsUpdate();
        }
        protected override void OnDamaged(int value)
        {
            SLevelController.PlayerDamaged(value);

            _ = SSounds.Play("Damage_10");
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
            _ = SSounds.Play("Explosion_10");

            SGameController.SetGameState(SGameState.GameOver);
            SLevelController.GameOver();

            _ = SEffectsManager.Create<SExplosionEffect>(WorldPosition);
        }
        public override void Reset()
        {
            HealthValue = 3;
            DamageValue = 1;

            ChanceOfKnockback = 0;
            KnockbackForce = 0;

            ShootDelay = 3f;
            BulletLifeTime = 3f;
            BulletSpeed = 3f;

            currentShootDelay = ShootDelay;
        }

        private void ShootUpdate()
        {
            if (ShootDelay <= 0)
            {
                this.currentShootDelay = 0;
                return;
            }

            if (this.currentShootDelay > 0)
            {
                this.currentShootDelay -= 0.1f;
                this.currentShootDelay = Math.Clamp(this.currentShootDelay, 0, ShootDelay);
                return;
            }
        }

        private void InputsUpdate()
        {
            PauseInputUpdate();
            MovementInputUpdate();
            ShootInputUpdate();
        }
        private static void PauseInputUpdate()
        {
            if (SInput.Started(Keys.P))
            {
                SGameController.SetGameState(SGameState.Paused);
            }
        }
        private void MovementInputUpdate()
        {
            if (SInput.Started(Keys.A) || SInput.Started(Keys.Left))
            {
                _ = SSounds.Play("Player_Movement");
                LocalPosition = new(LocalPosition.X - 1, LocalPosition.Y);
            }

            if (SInput.Started(Keys.D) || SInput.Started(Keys.Right))
            {
                _ = SSounds.Play("Player_Movement");
                LocalPosition = new(LocalPosition.X + 1, LocalPosition.Y);
            }
        }
        private void ShootInputUpdate()
        {
            if (SInput.Performed(Keys.Space))
            {
                if (!CanShoot)
                {
                    return;
                }

                _ = SSounds.Play("Shoot_01");

                this.currentShootDelay = ShootDelay;

                SProjectileManager.Create(new()
                {
                    SpriteId = 0,
                    Team = Teams.Good,
                    Position = new(WorldPosition.X, WorldPosition.Y - 32f),
                    Speed = new(0, BulletSpeed * -1),
                    Damage = DamageValue,
                    LifeTime = BulletLifeTime,
                    Range = 10f
                });
            }
        }
    }
}