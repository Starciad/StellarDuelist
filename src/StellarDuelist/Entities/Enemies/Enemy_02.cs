using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Game.Enums;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN SPACESHIP ]
    /// </summary>
    /// <remarks>
    /// Moves downwards interspersed with horizontal movements based on a few pre-determined seconds.
    /// <br/><br/>
    /// Shoots lasers that move steadily downwards. They hurt the <see cref="SPlayerEntity"/>.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed partial class Enemy_02 : SEnemyEntity
    {
        #region Definition
        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultyRate >= 2;
                });
            }
        }
        #endregion

        // ==================================================== //

        private const float SHOOT_SPEED = 2f;
        private const float SHOOT_LIFE_TIME = 25f;

        private readonly STimer actionTimer = new(10f);

        private Direction movementDirection;
        private bool action;

        // ==================================================== //
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 1));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 1));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 3;
            this.AttackValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }
        protected override void OnStart()
        {
            this.actionTimer.Restart();
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
            ActionUpdate();
        }

        // UPDATE
        private void TimersUpdate()
        {
            this.actionTimer.Update();
        }
        private void ActionUpdate()
        {
            if (this.actionTimer.IsFinished)
            {
                this.actionTimer.Restart();

                // Action (true) = Random Movement
                // Action (false) = Shoot

                if (action)
                {
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
                else
                {
                    Shoot();
                }

                action = !action;
            }
        }

        // SKILLS
        private void Shoot()
        {
            _ = SSounds.Play("Shoot_01");
            SProjectileManager.Create(new()
            {
                SpriteId = 0,
                Team = STeam.Bad,
                Position = new(this.WorldPosition.X, this.WorldPosition.Y + 16f),
                Speed = new(0, SHOOT_SPEED),
                Damage = this.AttackValue,
                LifeTime = SHOOT_LIFE_TIME,
                Range = 10
            });
        }
    }
}
