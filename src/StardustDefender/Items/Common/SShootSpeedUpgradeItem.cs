using StardustDefender.Controllers;
using StardustDefender.Engine;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SShootSpeedUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            Animation.SetTexture(STextures.GetTexture("Upgrades"));
            Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 4, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.ShootSpeed += 0.1f;
        }
    }
}
