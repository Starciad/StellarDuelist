using StardustDefender.Engine;
using StardustDefender.Enums;

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
        public override void Reset()
        {
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
    }
}