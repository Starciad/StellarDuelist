using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Managers;
using StardustDefender.Effects.Common;

using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using StardustDefender.Controllers;

namespace StardustDefender.Entities.Aliens
{
    internal class SAlienEntity : SAlien
    {
        private readonly float movementDelay = 10f;
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
            SSounds.Play("Damage_02");
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
            SLevelController.EnemyKilled();

            SSounds.Play("Explosion_01");
            SEffectsManager.Create<SExplosionEffect>(WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
                SItemsManager.GetRandomItem(WorldPosition);
        }

        private void MovementUpdate()
        {
            if (currentMovementDelay < movementDelay)
            {
                currentMovementDelay += 0.1f;
            }
            else
            {
                currentMovementDelay = 0;
                switch (movementDirection)
                {
                    case 1:
                        int direction = SRandom.Chance(50, 100) ? -1 : 1;
                        LocalPosition = new(LocalPosition.X + direction, LocalPosition.Y);
                        movementDirection = 2;
                        break;

                    case 2:
                        LocalPosition = new(LocalPosition.X, LocalPosition.Y + 1);
                        movementDirection = 1;
                        break;
                }
            }
        }

        public override void Reset()
        {
            Animation.Reset();
            Animation.SetMode(AnimationMode.Forward);
            Animation.SetTexture(STextures.GetTexture("Aliens"));
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
