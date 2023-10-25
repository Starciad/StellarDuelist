using Microsoft.Xna.Framework.Graphics;

namespace StardustDefender.Core.Components
{
    public static class SFonts
    {
        public static SpriteFont Impact { get; private set; }

        internal static void Load()
        {
            Impact = SContent.Fonts.Load<SpriteFont>("Impact");
        }
    }
}