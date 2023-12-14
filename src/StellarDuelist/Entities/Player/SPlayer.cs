using Microsoft.Xna.Framework;

using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Utilities;
using StellarDuelist.Core.World;

namespace StellarDuelist.Game.Entities.Player
{
    [SEntityRegister(typeof(Definition))]
    internal sealed partial class SPlayer : SPlayerEntity
    {
        #region Definition
        private sealed class Definition : SEntityDefinition
        {
            protected override void OnBuild()
            {
                this.classification = SEntityClassification.Player;
            }
        }
        #endregion

        // ==================================================== //

        private readonly STimer invincibilityTimer = new(10f);
        private bool isHurt;

        // ==================================================== //
        // SYSTEM
        public override void Reset()
        {
            base.Reset();

            // Animations
            this.Animation.Reset();
            this.Animation.ClearFrames();
            this.Animation.SetTexture(STextures.GetTexture("PLAYER_Spaceship"));
            this.Animation.AddFrame(STextures.GetSprite(32, 0, 0));

            // Team
            this.Team = STeam.Good;

            // Attributes
            this.HealthValue = 3;
            this.AttackValue = 1;
            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;

            this.ShootDelay = 3f;
            this.BulletLifeTime = 3f;
            this.BulletSpeed = 3f;

            this.isHurt = false;
            this.IsInvincible = false;
            this.invincibilityTimer.Stop();

            // Timers
            this.ShootTimer.SetDelay(this.ShootDelay);
        }
        protected override void OnAwake()
        {
            this.OnDamaged += OnDamaged_Entity;
            this.OnDamaged += OnDamaged_Effects;
            this.OnDamaged += OnDamaged_Colors;
            this.OnDestroyed += OnDestroyed_Entity;
            this.OnDestroyed += OnDestroyed_Effects;
            this.OnDestroyed += OnDestroyed_System;
            this.OnDestroyed += OnDestroyed_Events;
        }
        protected override void OnStart()
        {
            this.ShootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            ClampUpdate();
            TimersUpdate();
            HurtUpdate();
            InputsUpdate();
        }

        // UPDATE
        private void ClampUpdate()
        {
            this.LocalPosition = SWorld.ClampVerticalPosition(this.LocalPosition);
        }
        private void TimersUpdate()
        {
            this.ShootTimer.Update();
        }
        private void HurtUpdate()
        {
            if (this.isHurt)
            {
                this.Color = Color.Red;
                this.invincibilityTimer.Update();

                if (this.invincibilityTimer.IsFinished)
                {
                    this.Color = Color.White;
                    this.isHurt = false;
                    this.IsInvincible = false;
                    this.invincibilityTimer.Stop();
                }
            }
        }
    }
}