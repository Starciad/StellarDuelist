using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Extensions;
using StardustDefender.Core.GUI;

using System.Text;

namespace StardustDefender.GUI
{
    internal sealed class GUI_Introduction : SGUI
    {
        // Textures
        private Texture2D logo;

        // Fonts
        private SpriteFont font;

        // Message
        private readonly StringBuilder S_Intro = new();
        private Vector2 S_IntroMeasure;

        protected override bool ConditionToBeDrawn()
        {
            return SGameController.State == SGameState.Introduction;
        }

        protected override void OnEnable()
        {
            SSongs.Play($"Opening_{SRandom.Range(1, 6)}");
        }

        protected override void OnInitialize()
        {
            this.logo = STextures.GetTexture("UI_Logo");
            this.font = SFonts.Impact;

            _ = this.S_Intro.Clear();
            _ = this.S_Intro.Append("Press Space to Continue!");

            this.S_IntroMeasure = this.font.MeasureString(this.S_Intro);
        }
        protected override void OnUpdate()
        {
            if (SInput.Started(Keys.Space))
            {
                SGameController.SetGameState(SGameState.Running);
                SLevelController.StartNewLevel();
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(this.logo, new Vector2(SCamera.Center.X, SCamera.Center.Y - 64), null, Color.White, 0f, this.logo.GetOriginPosition(), new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Intro, new Vector2(SCamera.Center.X + 110, SCamera.Center.Y + 128), Color.White, 0f, this.S_IntroMeasure, 0.5f, SpriteEffects.None, 0f);
        }
    }
}