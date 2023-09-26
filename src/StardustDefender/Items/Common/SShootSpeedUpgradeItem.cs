using StardustDefender.Core;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SShootSpeedUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 4, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.BulletSpeed += 0.1f;
        }
    }
}
