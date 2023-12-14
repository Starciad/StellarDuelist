using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Extensions;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Game.Effects;

using System.Linq;
using System.Threading.Tasks;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN DEFENSE FLAMES ]
    /// </summary>
    /// <remarks>
    /// If there are aliens in the scenario, he randomly chooses a target to position himself around it in a 3x3 area.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed class Enemy_06 : SEnemyEntity
    {
        // ==================================================== //

        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultyRate >= 11;
                });
            }
        }

        // ==================================================== //

        private readonly STimer movementTimer = new(5f);
        private SEntity targetToBeDefended;

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            base.Reset();

            this.movementTimer.Start();

            this.Animation.Reset();
            this.Animation.ClearFrames();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 5));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 5));
            this.Animation.SetDuration(1f);

            this.Team = STeam.Bad;

            this.HealthValue = 35;
            this.AttackValue = 2;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        // OVERRIDE
        protected override void OnStart()
        {
            this.movementTimer.Restart();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            TimersUpdate();

            // Collision
            if (SEntityUtilities.IsColliding(this, SLevelController.Player))
            {
                SLevelController.Player.Damage(1);
                Destroy();
            }

            // AI
            MovementUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_08");
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
            SLevelController.EnemyKilled();
            this.movementTimer.Stop();

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(15, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
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

            if (this.targetToBeDefended == null || this.targetToBeDefended.IsDestroyed)
            {
                ChooseNewRandomTargetToDefend();
            }

            Vector2 targetPosition = this.targetToBeDefended.LocalPosition;
            this.LocalPosition = new(targetPosition.X + SRandom.Range(-1, 2), targetPosition.Y + SRandom.Range(0, 2));

            this.movementTimer.Restart();
        }

        // SKILLS
        private void ChooseNewRandomTargetToDefend()
        {
            this.targetToBeDefended = SEntityManager.ActiveEntities.Where(x => x.Team == STeam.Bad).SelectRandom();
        }
    }
}