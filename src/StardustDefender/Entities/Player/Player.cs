using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Entities;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Player
{
    [SEntityRegister(typeof(Header))]
    internal sealed class Player : SPlayerEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Player;
            }
        }

        // ==================================================== //

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            this.ShootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();
            InputsUpdate();
        }
        protected override void OnDamaged(int value)
        {
            SLevelController.PlayerDamaged(value);

            _ = SSounds.Play("Damage_10");
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
            _ = SSounds.Play("Explosion_10");

            SGameController.SetGameState(SGameState.GameOver);
            SLevelController.GameOver();

            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);
        }

        public override void Reset()
        {
            // Animations
            this.Animation.Reset();
            this.Animation.Clear();
            this.Animation.SetTexture(STextures.GetTexture("PLAYER_Spaceship"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 0));

            // Team
            this.Team = STeam.Good;

            // Attributes
            this.HealthValue = 3;
            this.DamageValue = 1;
            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;

            this.ShootDelay = 3f;
            this.BulletLifeTime = 3f;
            this.BulletSpeed = 3f;

            // Timers
            this.ShootTimer.SetDelay(this.ShootDelay);
        }

        private void TimersUpdate()
        {
            this.ShootTimer.Update();
        }

        #region INPUTS
        private void InputsUpdate()
        {
            PauseInputUpdate();
            MovementInputUpdate();
            ShootInputUpdate();

#if DEBUG
            DEBUG_Kill_Enemies();
#endif
        }

#if DEBUG
        private void DEBUG_Kill_Enemies()
        {
            if (SInput.Started(Keys.D0))
            {
                foreach (SEntity entity in SEntityManager.Entities)
                {
                    if (entity == this)
                        continue;

                    entity.Destroy();
                }
            }
        }
#endif
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
                this.LocalPosition = new(this.LocalPosition.X - 1, this.LocalPosition.Y);
            }

            if (SInput.Started(Keys.D) || SInput.Started(Keys.Right))
            {
                _ = SSounds.Play("Player_Movement");
                this.LocalPosition = new(this.LocalPosition.X + 1, this.LocalPosition.Y);
            }
        }
        private void ShootInputUpdate()
        {
            if (!this.CanShoot)
            {
                return;
            }

            if (SInput.Performed(Keys.Space) || SInput.Performed(Keys.K))
            {
                this.ShootTimer.Restart();
                _ = SSounds.Play("Shoot_01");

                SProjectileManager.Create(new()
                {
                    SpriteId = 0,
                    Team = STeam.Good,
                    Position = new(this.WorldPosition.X, this.WorldPosition.Y - 32f),
                    Speed = new(0, this.BulletSpeed * -1),
                    Damage = this.DamageValue,
                    LifeTime = this.BulletLifeTime,
                    Range = 10f
                });
            }
        }
        #endregion
    }
}