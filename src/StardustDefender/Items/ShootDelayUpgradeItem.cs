using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Items;

using System;

namespace StardustDefender.Items
{
    internal sealed class ShootDelayUpgradeItem : SItemRegister
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 2, 0));
        }
        protected override void OnEffect(SPlayerEntity player)
        {
            player.ShootDelay -= 0.1f;
            player.ShootDelay = Math.Clamp(player.ShootDelay, 0.1f, 100f);
        }
    }
}
