using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Extensions;

namespace StardustDefender.Core.Components
{
    public static class SFade
    {
        private static Texture2D _fadeTexture;
        private static Vector2 _position;
        private static Vector2 _origin;
        private static Color _color;

        private static Color currentColor;
        private static float fadeLerp = 0.2f;
        private static int fadeTarget = 2;

        internal static void Load()
        {
            _fadeTexture = STextures.GetTexture("UI_SolidBackground");
            _position = SCamera.Center;
            _origin = _fadeTexture.GetOriginPosition();
            _color = Color.Transparent;
        }
        internal static void Update()
        {
            switch (fadeTarget)
            {
                // Fade In
                case 1:
                    currentColor = Color.Lerp(currentColor, _color, fadeLerp);
                    break;

                // Fade Out
                case 2:
                    currentColor = Color.Lerp(currentColor, Color.Transparent, fadeLerp);
                    break;
            }
        }
        internal static void Draw()
        {
            SGraphics.SpriteBatch.Draw(_fadeTexture, _position, null, currentColor, 0f, _origin, 1f, SpriteEffects.None, 0f);
        }

        public static void FadeIn(Color color, float lerp)
        {
            _color = color;
            fadeLerp = lerp;
            fadeTarget = 1;
        }
        public static void FadeOut(float lerp)
        {
            fadeLerp = lerp;
            fadeTarget = 2;
        }
    }
}
