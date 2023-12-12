using Microsoft.Xna.Framework.Graphics;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// Static collection for all fonts used in the game.
    /// </summary>
    public static class SFonts
    {
        /// <summary>
        /// The Impact font.
        /// </summary>
        public static SpriteFont Impact { get; private set; }

        /// <summary>
        /// Load all the fonts to be used in the game.
        /// </summary>
        internal static void Load()
        {
            Impact = SContent.Fonts.Load<SpriteFont>("Impact");
        }
    }
}