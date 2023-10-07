using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Engine;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Enemies
{
    [SEntityRegister(typeof(Header))]
    internal sealed class Enemy_05 : SEnemyEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Enemy;
            }

            protected override bool OnSpawningCondition()
            {
                return SDifficultyController.DifficultyRate >= 10;
            }
        }

        // ==================================================== //

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 4));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 4));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 25;
            this.DamageValue = 2;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        // OVERRIDE
        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnUpdate()
        {
            // Behaviour
            CollideWithPlayer();

            // AI (Move + Shoot)

        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_05");
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

            _ = SSounds.Play("Explosion_04");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(15, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
        }

        // UPDATE


        // SKILLS
    }
}
