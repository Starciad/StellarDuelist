using StardustDefender.Core;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SShootDelayUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 2, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.ShootDelay -= 0.1f;
        }
    }
}
