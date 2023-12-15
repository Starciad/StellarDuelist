using Microsoft.Xna.Framework;

using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Entities.Utilities;
using StellarDuelist.Core.Managers;
using StellarDuelist.Core.SEventArgs.Entities;
using StellarDuelist.Game.Effects;

namespace StellarDuelist.Game.Entities.Bosses
{
    internal sealed partial class Boss_01
    {
        protected override void OnSubscribeEvents()
        {
            this.OnDamaged += OnDamaged_Effects;
            this.OnDamaged += OnDamaged_Colors;
            this.OnDestroyed += OnDestroyed_Entity;
            this.OnDestroyed += OnDestroyed_Effects;
            this.OnDestroyed += OnDestroyed_Drops;
        }

        protected override void OnUnsubscribeEvents()
        {
            this.OnDamaged -= OnDamaged_Effects;
            this.OnDamaged -= OnDamaged_Colors;
            this.OnDestroyed -= OnDestroyed_Entity;
            this.OnDestroyed -= OnDestroyed_Effects;
            this.OnDestroyed -= OnDestroyed_Drops;
        }

        #region DAMAGED (EVENTS)
        private void OnDamaged_Effects(SEntityDamagedEventArgs e)
        {
            _ = SSounds.Play("Damage_05");
            _ = SEffectsManager.Create<ImpactEffect>(this.WorldPosition, new(2f));
        }
        private void OnDamaged_Colors(SEntityDamagedEventArgs e)
        {
            SEntityEffectsUtilities.ApplyTemporaryDamageColor(this, 235);
        }
        #endregion

        #region DESTROYED (EVENTS)
        private void OnDestroyed_Entity()
        {
            SLevelController.BossKilled();
        }
        private void OnDestroyed_Effects()
        {
            _ = SSounds.Play("Explosion_05");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition, new(2f));
        }
        private void OnDestroyed_Drops()
        {
            int[] offsets = { 32, -32 };

            for (int i = 0; i < offsets.Length; i++)
            {
                for (int j = 0; j < offsets.Length; j++)
                {
                    int offsetX = offsets[i];
                    int offsetY = offsets[j];

                    if (offsetX == 0 && offsetY == 0)
                    {
                        continue;
                    }

                    Vector2 dropPosition = new(this.WorldPosition.X + offsetX, this.WorldPosition.Y + offsetY);
                    _ = SItemsManager.CreateRandomItem(dropPosition);
                }
            }
        }
        #endregion
    }
}
