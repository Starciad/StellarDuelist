using Microsoft.Xna.Framework;

namespace StellarDuelist.Core.Colors
{
    /// <summary>
    /// A static class that provides color palettes for various purposes.
    /// </summary>
    public static class SPalettes
    {
        /// <summary>
        /// Gets an array of colors representing a warning palette.
        /// </summary>
        public static Color[] WARNING_PALETTE { get; } = new Color[] {
            Color.Yellow,
            Color.LightYellow,
            Color.Orange,
            Color.MonoGameOrange,
            Color.White,
        };
    }
}
