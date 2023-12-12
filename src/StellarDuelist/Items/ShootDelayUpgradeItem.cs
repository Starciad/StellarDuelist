using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Items;

using System;

namespace StellarDuelist.Game.Items
{
    /// <summary>
    /// Upgrade item for shortening the delay of the player's shots.
    /// </summary>
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