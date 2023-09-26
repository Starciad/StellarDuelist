using StardustDefender.Core;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SHealthUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 0, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.HealthValue += 1;
        }
    }
}
