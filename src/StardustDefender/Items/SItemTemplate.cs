using Microsoft.Xna.Framework;

using StardustDefender.Animation;
using StardustDefender.Controllers;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items
{
    internal abstract class SItemTemplate
    {
        internal SAnimation Animation { get; private set; } = new();

        internal static Color[] COLOR_PALETTE = new Color[] {
            Color.Yellow,
            Color.LightYellow,
            Color.Orange,
            Color.MonoGameOrange,
            Color.White,
        };
        internal const int SPRITE_SCALE = 16;

        internal void Initialize()
        {
            OnInitialize();
            this.Animation.Initialize();
        }
        internal void ApplyEffect()
        {
            OnEffect(SLevelController.Player);
        }

        protected abstract void OnInitialize();
        protected abstract void OnEffect(SPlayerEntity player);
    }
}
