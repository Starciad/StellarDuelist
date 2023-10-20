using Microsoft.Xna.Framework;

namespace StardustDefender.Core.Components
{
    public static class STime
    {
        public static GameTime UpdateTime { get; private set; }
        public static GameTime DrawTime { get; private set; }

        internal static void SetUpdateGameTime(GameTime value)
        {
            UpdateTime = value;
        }

        internal static void SetDrawGameTime(GameTime value)
        {
            DrawTime = value;
        }
    }
}
