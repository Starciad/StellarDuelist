using StardustDefender.Engine;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SAttackUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 1, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.DamageValue += 1;
        }
    }
}
