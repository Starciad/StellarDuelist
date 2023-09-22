﻿using Microsoft.Xna.Framework.Graphics;

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