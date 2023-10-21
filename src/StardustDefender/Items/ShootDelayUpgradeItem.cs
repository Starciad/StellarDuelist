using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Items;

using System;

namespace StardustDefender.Game.Items
{
    internal sealed class ShootDelayUpgradeItem : SItemRegister
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddFrame(STextures.GetSprite(SPRITE_SCALE, 2, 0));
        }
        protected override void OnEffect(SPlayerEntity player)
        {
            player.ShootDelay -= 0.1f;
            player.ShootDelay = Math.Clamp(player.ShootDelay, 0.1f, 100f);
        }

        protected override bool SpawnCondition(SPlayerEntity player)
        {
            return player.ShootDelay > 0.5f;
        }
    }
}