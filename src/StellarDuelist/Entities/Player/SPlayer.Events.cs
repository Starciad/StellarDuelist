using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.SEventArgs.Entities;
using StellarDuelist.Game.Effects;

namespace StellarDuelist.Game.Entities.Player
{
    internal sealed partial class SPlayer
    {
        #region DAMAGED (EVENTS)
        private void OnDamaged_Entity(SEntityDamagedEventArgs e)
        {
            SLevelController.PlayerDamaged(e.DamageAmount);

            this.isHurt = true;
            this.IsInvincible = true;
            this.invincibilityTimer.Restart();
        }
        private void OnDamaged_Effects(SEntityDamagedEventArgs e)
        {
            _ = SSounds.Play("Damage_10");
            _ = SEffectsManager.Create<ImpactEffect>(this.WorldPosition);
        }
        private void OnDamaged_Colors(SEntityDamagedEventArgs e)
        {
            SEntityEffectsUtilities.ApplyTemporaryDamageColor(this, 235);
        }
        #endregion

        #region DESTROYED (EVENTS)
        private void OnDestroyed_Entity()
        {
            this.invincibilityTimer.Stop();
        }
        private void OnDestroyed_Effects()
        {
            _ = SSounds.Play("Explosion_10");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);
        }
        private void OnDestroyed_System()
        {
            SGameController.SetGameState(SGameState.GameOver);
            SLevelController.GameOver();
        }
        private void OnDestroyed_Events()
        {
            this.OnDamaged -= OnDamaged_Entity;
            this.OnDamaged -= OnDamaged_Effects;
            this.OnDamaged -= OnDamaged_Colors;
            this.OnDestroyed -= OnDestroyed_Entity;
            this.OnDestroyed -= OnDestroyed_Effects;
            this.OnDestroyed -= OnDestroyed_System;
            this.OnDestroyed -= OnDestroyed_Events;
        }
        #endregion
    }
}
