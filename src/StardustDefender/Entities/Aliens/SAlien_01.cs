using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;

using Microsoft.Xna.Framework;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Aliens
{
    internal sealed class SAlien_01 : SAlienEntity
    {
        private readonly STimer movementTimer = new(10f);
        private int movementDirection = 1;

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            movementTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();

            // Behaviour
            CollideWithPlayer();

            // AI
            MovementUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_02");
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
            SLevelController.EnemyKilled();
            movementTimer.Stop();

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<SExplosionEffect>(WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.CreateRandomItem(WorldPosition);
            }
        }
        public override void Reset()
        {
            movementTimer.Start();

            Animation.Reset();
            Animation.Clear();

            Animation.SetMode(AnimationMode.Forward);
            Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            Animation.AddSprite(STextures.GetSprite(32, 0, 0));
            Animation.AddSprite(STextures.GetSprite(32, 1, 0));
            Animation.SetDuration(3f);

            Team = Teams.Bad;

            HealthValue = 2;
            DamageValue = 1;

            ChanceOfKnockback = 50;
            KnockbackForce = 1;
        }

        private void TimersUpdate()
        {
            movementTimer.Update();
        }
        private void MovementUpdate()
        {
            if (!this.movementTimer.IsFinished)
            {
                return;
            }

            this.movementTimer.Restart();
            switch (this.movementDirection)
            {
                case 1:
                    int direction = SRandom.Chance(50, 100) ? -1 : 1;
                    LocalPosition = new(LocalPosition.X + direction, LocalPosition.Y);
                    this.movementDirection = 2;
                    break;

                case 2:
                    LocalPosition = new(LocalPosition.X, LocalPosition.Y + 1);
                    this.movementDirection = 1;
                    break;
            }
        }
    }
}