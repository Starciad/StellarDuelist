using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Templates.Dangerous;
using StellarDuelist.Core.Enums;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ EATER ]
    /// </summary>
    /// <remarks>
    /// Moves constantly downwards and to the sides. Changes horizontal direction when it bounces off the edges of the screen. It has a high speed.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed partial class Enemy_04 : SEnemyEntity
    {
        #region Definition
        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultySettings.DifficultyRate >= 7;
                });
            }
        }
        #endregion

        // ==================================================== //

        private const float HORIZONTAL_SPEED = 0.05f;
        private const float VERTICAL_SPEED = 0.01f;

        private bool horizontalDirection;
        private Vector2 previousLocalPosition;

        // ==================================================== //
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 3));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 3));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 15;
            this.AttackValue = 2;

            this.ChanceOfKnockback = 25;
            this.KnockbackForce = 2;
        }
        protected override void OnUpdate()
        {
            // Collision
            if (IsCollidingWithThePlayer())
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI
            HorizontalMovementUpdate();
            VerticalMovementUpdate();
            this.previousLocalPosition = this.LocalPosition;
        }

        // UPDATE
        private void HorizontalMovementUpdate()
        {
            // MOVING
            this.LocalPosition = this.horizontalDirection
                ? new(this.LocalPosition.X + HORIZONTAL_SPEED, this.LocalPosition.Y)
                : new(this.LocalPosition.X - HORIZONTAL_SPEED, this.LocalPosition.Y);

            if (this.previousLocalPosition.X == this.LocalPosition.X)
            {
                this.horizontalDirection = !this.horizontalDirection;
            }
        }
        private void VerticalMovementUpdate()
        {
            // MOVING
            this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + VERTICAL_SPEED);
        }
    }
}
