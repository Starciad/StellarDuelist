﻿using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Items;

namespace StardustDefender.Items
{
    internal sealed class ShootSpeedUpgradeItem : SItemRegister
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