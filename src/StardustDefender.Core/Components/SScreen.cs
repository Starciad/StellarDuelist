using Microsoft.Xna.Framework.Graphics;

namespace StardustDefender.Core.Components
{
    public static class SScreen
    {
        public static int Width => 1280;
        public static int Height => 720;
        public static Viewport Viewport { get; private set; }

        internal static void Initialize(Viewport value)
        {
            Viewport = value;
        }
    }
}
