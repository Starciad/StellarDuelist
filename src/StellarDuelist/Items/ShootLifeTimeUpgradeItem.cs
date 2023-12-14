using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Templates;
using StellarDuelist.Core.Items;
using StellarDuelist.Game.Entities.Player;

namespace StellarDuelist.Game.Items
{
    /// <summary>
    /// Upgrade item to increase the durability of the player's bullets.
    /// </summary>
    internal sealed class ShootLifeTimeUpgradeItem : SItemDefinition
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
