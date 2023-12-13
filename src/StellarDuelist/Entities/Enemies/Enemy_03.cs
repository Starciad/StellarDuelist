using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Game.Effects;

using System;
using System.Threading.Tasks;

namespace StellarDuelist.Game.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN STAR ]
    /// </summary>
    /// <remarks>
    /// Continuously moves downwards. When he dies, he releases a barrage of projectiles that are launched in a 360 degree radius around his point of death.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Definition))]
    internal sealed class Enemy_03 : SEntity
    {
        // ==================================================== //

        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Enemy;
                this.canSpawn = new(() =>
                {
                    return SDifficultyController.DifficultyRate >= 5;
                });
            }
        }

        // ==================================================== //

        private const float SPEED = 0.5f;

        // Bullets
        private const float BULLET_SPEED = 1f;
        private const float BULLET_LIFE_TIME = 50f;

        private const int NUMBER_OF_BULLETS = 12;
        private const float SPREAD_ANGLE_DEGRESS = 360f;

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            base.Reset();

            this.Animation.Reset();
            this.Animation.ClearFrames();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 2));
            this.Animation.AddFrame(STextures.GetSprite(32, 1, 2));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 6;
            this.AttackValue = 2;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        // OVERRIDE
        protected override void OnUpdate()
        {
            base.OnUpdate();

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
            _ = SSounds.Play("Damage_04");
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

            _ = SSounds.Play("Explosion_02");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }

            // Shoot
            FlurryOfShots();
        }

        // UPDATE
        private void MovementUpdate()
        {
            this.WorldPosition = new(this.WorldPosition.X, this.WorldPosition.Y + SPEED);
        }

        // SKILLS
        private void FlurryOfShots()
        {
            _ = SSounds.Play("Explosion_03");

            float angleIncrement = SPREAD_ANGLE_DEGRESS / (NUMBER_OF_BULLETS - 1);
            float currentAngle = SRandom.Range(0, 361);

            for (int i = 0; i < NUMBER_OF_BULLETS; i++)
            {
                float radians = MathHelper.ToRadians(currentAngle);
                Vector2 direction = new((float)Math.Cos(radians), (float)Math.Sin(radians));

                SProjectileManager.Create(new()
                {
                    SpriteId = 2,
                    Team = STeam.Bad,
                    Position = new(this.WorldPosition.X, this.WorldPosition.Y),
                    Speed = new(BULLET_SPEED * direction.X, BULLET_SPEED * direction.Y),
                    Damage = this.AttackValue,
                    LifeTime = BULLET_LIFE_TIME,
                    Range = 3
                });

                currentAngle += angleIncrement;
            }
        }
    }
}
