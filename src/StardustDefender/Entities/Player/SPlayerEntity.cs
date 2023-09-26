using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Player
{
    internal sealed class SPlayerEntity : SEntity
    {
        public bool CanShoot => this.shootTimer.IsFinished;
        public float BulletLifeTime { get; set; }
        public float BulletSpeed { get; set; }
        public float ShootDelay
        {
            get => this.shootDelay;

            set
            {
                this.shootDelay = value;
                this.shootTimer.SetDelay(value);
            }
        }

        private readonly STimer shootTimer = new();
        private float shootDelay;

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            this.shootTimer.Restart();
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
            _ = SEffectsManager.Create<SImpactEffect>(this.WorldPosition);

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

            _ = SEffectsManager.Create<SExplosionEffect>(this.WorldPosition);
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
            this.shootTimer.SetDelay(this.ShootDelay);
        }

        private void TimersUpdate()
        {
            this.shootTimer.Update();
        }

        #region INPUTS
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

            if (SInput.Performed(Keys.Space))
            {
                this.shootTimer.Restart();
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