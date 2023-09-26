using StardustDefender.Core;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SAttackUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 1, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.DamageValue += 1;
        }
    }
}
