using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Game.Enums;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN ]
    /// </summary>
    /// <remarks>
    /// Moves downwards interspersed with horizontal movements based on a few pre-determined seconds.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed partial class Enemy_01 : SEnemyEntity
    {
        #region Definition
        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() => { return true; });
            }
        }
        #endregion

        // ==================================================== //

        private readonly STimer movementTimer = new(5f);
        private Direction movementDirection;

        // ==================================================== //
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            this.movementTimer.Start();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 0));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 0));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 2;
            this.AttackValue = 1;

            this.ChanceOfKnockback = 50;
            this.KnockbackForce = 1;
        }
        protected override void OnStart()
        {
            this.movementTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();

            // Collision
            if (SEntityCollisionUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI
            MovementUpdate();
        }

        // UPDATE
        private void TimersUpdate()
        {
            this.movementTimer.Update();
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
                case Direction.Horizontal:
                    int direction = SRandom.Chance(50, 100) ? -1 : 1;
                    this.LocalPosition = new(this.LocalPosition.X + direction, this.LocalPosition.Y);
                    this.movementDirection = Direction.Vertical;
                    break;

                case Direction.Vertical:
                    this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + 1);
                    this.movementDirection = Direction.Horizontal;
                    break;
            }
        }
    }
}