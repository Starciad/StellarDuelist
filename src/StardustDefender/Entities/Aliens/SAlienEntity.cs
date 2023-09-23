using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Effects.Common;
using StardustDefender.Engine;

using StardustDefender.Enums;
using StardustDefender.Managers;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Aliens
{
    internal class SAlienEntity : SAlien
    {
        private readonly float movementDelay = 0.5f; // 10f
        private float currentMovementDelay = 0f;
        private int movementDirection = 1;

        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnUpdate()
        {
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

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<SExplosionEffect>(WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.GetRandomItem(WorldPosition);
            }
        }

        private void MovementUpdate()
        {
            if (this.currentMovementDelay < this.movementDelay)
            {
                this.currentMovementDelay += 0.1f;
            }
            else
            {
                this.currentMovementDelay = 0;
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

        public override void Reset()
        {
            Animation.Reset();
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
    }
}
