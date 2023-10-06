using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core.Animation;
using StardustDefender.Core.Entities.Templates;

namespace StardustDefender.Core.Items
{
    public abstract class SItemRegister
    {
        public SAnimation Animation { get; private set; } = new();

        internal static Color[] COLOR_PALETTE = new Color[] {
            Color.Yellow,
            Color.LightYellow,
            Color.Orange,
            Color.MonoGameOrange,
            Color.White,
        };

        // Const values
        public const byte SPRITE_SCALE = 16;

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
