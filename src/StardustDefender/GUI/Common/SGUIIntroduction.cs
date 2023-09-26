using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Core;
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
        private readonly StringBuilder S_Intro = new();
        private Vector2 S_IntroMeasure;

        protected override void OnInitialize()
        {
            this.logo = STextures.GetTexture("UI_Logo");
            this.font = SFonts.Impact;

            this.S_Intro.Clear();
            this.S_Intro.Append("Press Space to Continue!");

            this.S_IntroMeasure = this.font.MeasureString(this.S_Intro);
        }
        protected override void OnUpdate()
        {
            if (SInput.Started(Keys.Space))
            {
                SGameController.SetGameState(SGameState.Running);
                SLevelController.RunLevel();
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(this.logo, new Vector2(SCamera.Center.X, SCamera.Center.Y - 64), null, Color.White, 0f, this.logo.GetOriginPosition(), new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Intro, new Vector2(SCamera.Center.X - (this.S_IntroMeasure.X / 1.5f), SCamera.Center.Y + 96), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        }
    }
}