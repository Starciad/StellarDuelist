using Microsoft.Xna.Framework;

using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Engine;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Game.Effects;
using StardustDefender.Game.Enums;

using System.Threading.Tasks;

namespace StardustDefender.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN ]
    /// </summary>
    /// <remarks>
    /// Moves downwards interspersed with horizontal movements based on a few pre-determined seconds.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Header))]
    internal sealed class Enemy_01 : SEnemyEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Enemy;
            }
        }

        // ==================================================== //

        private readonly STimer movementTimer = new(10f);

        private Direction movementDirection;

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            this.movementTimer.Start();

            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 0));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 0));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 2;
            this.DamageValue = 1;

            this.ChanceOfKnockback = 50;
            this.KnockbackForce = 1;
        }

        // OVERRIDE
        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            this.movementTimer.Restart();
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
            if (SRandom.Chance(20, 100))
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