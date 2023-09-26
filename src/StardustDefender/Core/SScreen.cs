using Microsoft.Xna.Framework.Graphics;

namespace StardustDefender.Core
{
    internal static class SScreen
    {
        internal static int Width => 1280;
        internal static int Height => 720;
        internal static Viewport Viewport { get; private set; }

        internal static void Initialize(Viewport value)
        {
            Viewport = value;
        }
    }
}
