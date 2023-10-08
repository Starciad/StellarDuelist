using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;

using System;
using System.Threading.Tasks;

namespace StardustDefender.Entities.Enemies
{
    /// <summary>
    /// [ ALIEN STAR ]
    /// </summary>
    /// <remarks>
    /// Continuously moves downwards. When he dies, he releases a barrage of projectiles that are launched in a 360 degree radius around his point of death.
    /// <br/><br/>
    /// Automatically dies when colliding with the <see cref="SPlayerEntity"/>.
    /// </remarks>
    [SEntityRegister(typeof(Header))]
    internal sealed class Enemy_03 : SEnemyEntity
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
                return SDifficultyController.DifficultyRate >= 5;
            }
        }

        // ==================================================== //

        private const float SPEED = 0.01f;

        // Bullets
        private const float BULLET_SPEED = 1f;
        private const float BULLET_LIFE_TIME = 50f;

        private const int NUMBER_OF_BULLETS = 12;
        private const float SPREAD_ANGLE_DEGRESS = 360f;

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 2));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 2));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 6;
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
            this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + SPEED);
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
                    Damage = this.DamageValue,
                    LifeTime = BULLET_LIFE_TIME,
                    Range = 7.5f
                });

                currentAngle += angleIncrement;
            }
        }
    }
}
