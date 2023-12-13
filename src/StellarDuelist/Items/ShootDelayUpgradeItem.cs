using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Items;
using StellarDuelist.Game.Entities.Player;

using System;

namespace StellarDuelist.Game.Items
{
    /// <summary>
    /// Upgrade item for shortening the delay of the player's shots.
    /// </summary>
    internal sealed class ShootDelayUpgradeItem : SItemDefinition
    {
        protected override void OnInitialize()
        {
            this.Animation.SetTexture(STextures.GetTexture("ITEMS_Upgrades"));
            this.Animation.AddFrame(STextures.GetSprite(SPRITE_SCALE, 2, 0));
        }

        protected override void OnEffect(SEntity player)
        {
            SPlayer p = (SPlayer)player;

            p.ShootDelay -= 0.1f;
            p.ShootDelay = Math.Clamp(p.ShootDelay, 0.1f, 100f);
        }

        protected override bool SpawnCondition(SEntity player)
        {
            return ((SPlayer)player).ShootDelay > 0.5f;
        }
    }
}