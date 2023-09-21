using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustDefender.Engine
{
    internal static class SFonts
    {
        internal static SpriteFont Impact { get; private set; }

        internal static void Load()
        {
            Impact = SContent.Fonts.Load<SpriteFont>("Impact");
        }
    }
}