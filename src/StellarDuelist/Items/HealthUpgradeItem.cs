using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Items;

namespace StellarDuelist.Game.Items
{
    /// <summary>
    /// Upgrade item for the player's health.
    /// </summary>
    internal sealed class HealthUpgradeItem : SItemDefinition
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddFrame(STextures.GetSprite(SPRITE_SCALE, 0, 0));
        }
        protected override void OnEffect(SPlayerEntity player)
        {
            player.HealthValue += 1;
        }
    }
}
