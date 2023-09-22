using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Engine;
using StardustDefender.Extensions;

using System.Text;

namespace StardustDefender.GUI.Common
{
    internal sealed class SGUIIntroduction : SGUI
    {
        // Textures
        private Texture2D logo;

        // Fonts
        private SpriteFont font;

        // Message
        private StringBuilder introString;
        private Vector2 introStringMeasure;

        protected override void OnInitialize()
        {
            this.logo = STextures.GetTexture("UI_Logo");
            this.font = SFonts.Impact;

            this.introString = new("Press Any Key to Continue!");
            this.introStringMeasure = this.font.MeasureString(this.introString);
        }
        protected override void OnUpdate()
        {
            if (SInput.Keyboard.GetPressedKeyCount() > 0)
            {
                SGameController.SetGameState(SGameState.Running);
                SLevelController.RunLevel();
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(this.logo, new Vector2(SCamera.Center.X, SCamera.Center.Y - 64), null, Color.White, 0f, this.logo.GetOriginPosition(), new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.introString, new Vector2(SCamera.Center.X - (this.introStringMeasure.X / 1.5f), SCamera.Center.Y + 96), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        }
    }
}