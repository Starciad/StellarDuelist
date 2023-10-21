﻿using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Items;

namespace StardustDefender.Game.Items
{
    internal sealed class ShootLifeTimeUpgradeItem : SItemRegister
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddFrame(STextures.GetSprite(SPRITE_SCALE, 3, 0));
        }
        protected override void OnEffect(SPlayerEntity player)
        {
            player.BulletLifeTime += 0.1f;
        }
    }
}
