using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;
using StardustDefender.Projectiles;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Player
{
    internal sealed class SPlayerEntity : SEntity
    {
        internal bool CanShoot => currentShootDelay == 0;
        public float ShootDelay { get; set; } = 2.5f;
        public float ShootLifeTime { get; set; } = 3f;
        public float ShootSpeed { get; set; } = 3f;

        private float currentShootDelay;

        protected override void OnAwake()
        {
            Animation.SetTexture(STextures.GetTexture("Player_1"));
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

            SSounds.Play("Damage_10");
            SEffectsManager.Create<SImpactEffect>(WorldPosition);

            _ = Task.Run(async () =>
            {
                Color = Color.Red;
                await Task.Delay(235);
                Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            SSounds.Play("Explosion_10");
            SGameController.SetGameState(SGameState.GameOver);
            SEffectsManager.Create<SExplosionEffect>(WorldPosition);
        }

        private void ShootUpdate()
        {
            if (ShootDelay <= 0)
            {
                currentShootDelay = 0;
                return;
            }

            if (currentShootDelay > 0)
            {
                currentShootDelay -= 0.1f;
                currentShootDelay = Math.Clamp(currentShootDelay, 0, ShootDelay);
                return;
            }
        }

        private void InputsUpdate()
        {
            MovementInputUpdate();
            ShootInputUpdate();
        }
        private void MovementInputUpdate()
        {
            if (SInput.Started(Keys.A) || SInput.Started(Keys.Left))
            {
                SSounds.Play("Player_Movement");
                LocalPosition = new(LocalPosition.X - 1, LocalPosition.Y);
            }

            if (SInput.Started(Keys.D) || SInput.Started(Keys.Right))
            {
                SSounds.Play("Player_Movement");
                LocalPosition = new(LocalPosition.X + 1, LocalPosition.Y);
            }
        }
        private void ShootInputUpdate()
        {
            if (SInput.Performed(Keys.Space))
            {
                if (!CanShoot)
                    return;

                SSounds.Play("Shoot_01");

                currentShootDelay = ShootDelay;

                SProjectileManager.Create(new()
                {
                    SpriteId = 0,
                    Team = Teams.Good,
                    Position = new(WorldPosition.X, WorldPosition.Y - 32f),
                    Speed = new(0, ShootSpeed * -1),
                    Damage = DamageValue,
                    LifeTime = ShootLifeTime,
                    Range = 10f
                });
            }
        }
    }
}