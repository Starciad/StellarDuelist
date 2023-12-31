﻿using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Items;

namespace StellarDuelist.Game.Items
{
    /// <summary>
    /// Upgrade item to increase the speed of the player's bullets.
    /// </summary>
    internal sealed class ShootSpeedUpgradeItem : SItemRegister
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddFrame(STextures.GetSprite(SPRITE_SCALE, 4, 0));
        }
        protected override void OnEffect(SPlayerEntity player)
        {
            player.BulletSpeed += 0.1f;
        }
    }
}
