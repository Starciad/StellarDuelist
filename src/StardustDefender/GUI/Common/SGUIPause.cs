using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Engine;
using StardustDefender.Extensions;

namespace StardustDefender.GUI.Common
{
    internal class SGUIPause : SGUI
    {
        // Textures
        private Texture2D backgroundTexture;
        private Texture2D pausedTexture;

        // Transform
        private Vector2 pausedTextureOrigin;
        private Vector2 backgroundTextureOrigin;

        protected override void OnInitialize()
        {
            backgroundTexture = STextures.GetTexture("UI_SolidBackground");
            pausedTexture = STextures.GetTexture("UI_Paused");

            pausedTextureOrigin = pausedTexture.GetOriginPosition();
            backgroundTextureOrigin = backgroundTexture.GetOriginPosition();
        }
        protected override void OnUpdate()
        {
            if (SInput.Started(Keys.P))
            {
                SGameController.SetGameState(SGameState.Running);
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(backgroundTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, new Color(1, 11, 25, 180), 0f, backgroundTextureOrigin, new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.Draw(pausedTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, Color.White, 0f, pausedTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);
        }
    }
}
