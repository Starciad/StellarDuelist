using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.SEventArgs.Entities;
using StellarDuelist.Game.Effects;

namespace StellarDuelist.Game.Entities.Enemies
{
    internal sealed partial class Enemy_06
    {
        #region DAMAGED (EVENTS)
        private void OnDamaged_Effects(SEntityDamagedEventArgs e)
        {
            _ = SSounds.Play("Damage_08");
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
            SLevelController.EnemyKilled();
            this.movementTimer.Stop();
        }
        private void OnDestroyed_Effects()
        {
            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);
        }
        private void OnDestroyed_Drops()
        {
            if (SRandom.Chance(15, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
        }
        private void OnDestroyed_Events()
        {
            this.OnDamaged -= OnDamaged_Effects;
            this.OnDamaged -= OnDamaged_Colors;
            this.OnDestroyed -= OnDestroyed_Entity;
            this.OnDestroyed -= OnDestroyed_Effects;
            this.OnDestroyed -= OnDestroyed_Drops;
            this.OnDestroyed -= OnDestroyed_Events;
        }
        #endregion
    }
}
