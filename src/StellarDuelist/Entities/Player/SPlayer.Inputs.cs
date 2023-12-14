using Microsoft.Xna.Framework.Input;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;

using System;

namespace StellarDuelist.Game.Entities.Player
{
    internal sealed partial class SPlayer
    {
        // ==================================================== //
        // UPDATE
        private void InputsUpdate()
        {
            PauseInputUpdate();
            MovementInputUpdate();
            ShootInputUpdate();

#if DEBUG
            DEBUG_Error();
            DEBUG_Increase_Power();
            DEBUG_Kill_Enemies();
#endif
        }

        // ==================================================== //
        // PLAYER INPUTS
        private static void PauseInputUpdate()
        {
            if (SInput.Started(Keys.P))
            {
                SGameController.SetGameState(SGameState.Paused);
            }
        }
        private void MovementInputUpdate()
        {
            if (SInput.Started(Keys.W) || SInput.Started(Keys.Up))
            {
                PlaySound();
                this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y - 1);
            }

            if (SInput.Started(Keys.S) || SInput.Started(Keys.Down))
            {
                PlaySound();
                this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + 1);
            }

            if (SInput.Started(Keys.A) || SInput.Started(Keys.Left))
            {
                PlaySound();
                this.LocalPosition = new(this.LocalPosition.X - 1, this.LocalPosition.Y);
            }

            if (SInput.Started(Keys.D) || SInput.Started(Keys.Right))
            {
                PlaySound();
                this.LocalPosition = new(this.LocalPosition.X + 1, this.LocalPosition.Y);
            }

            void PlaySound()
            {
                _ = SSounds.Play("Player_Movement");
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
                    Position = new(this.CurrentPosition.X, this.CurrentPosition.Y - 32f),
                    Speed = new(0, this.BulletSpeed * -1),
                    Damage = this.AttackValue,
                    LifeTime = this.BulletLifeTime,
                    Range = 10
                });
            }
        }

        // ==================================================== //
        // DEBUG INPUTS
#if DEBUG
        private static void DEBUG_Error()
        {
            if (SInput.Started(Keys.F5))
            {
                throw new Exception("Player - Experimental Exception.");
            }
        }
        private void DEBUG_Increase_Power()
        {
            if (SInput.Started(Keys.D1))
            {
                this.HealthValue += 1;
            }

            if (SInput.Started(Keys.D2))
            {
                this.AttackValue += 1;
            }

            if (SInput.Started(Keys.D3))
            {
                this.ShootDelay -= 0.1f;
                this.ShootDelay = Math.Clamp(this.ShootDelay, 0.1f, 100f);
            }

            if (SInput.Started(Keys.D4))
            {
                this.BulletLifeTime += 0.1f;
            }

            if (SInput.Started(Keys.D5))
            {
                this.BulletSpeed += 0.1f;
            }
        }
        private void DEBUG_Kill_Enemies()
        {
            if (SInput.Started(Keys.D0))
            {
                foreach (SEntity entity in SEntityManager.ActiveEntities)
                {
                    if (entity == this)
                    {
                        continue;
                    }

                    entity.Destroy();
                }
            }
        }
#endif

    }
}