using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Extensions;

using System;
using System.Text;

namespace StardustDefender.GUI.Common
{
    internal sealed class SGUIGameOver : SGUI
    {
        // Textures
        private Texture2D backgroundTexture;
        private Texture2D gameOverTexture;

        // Transform
        private Vector2 backgroundTextureOrigin;
        private Vector2 gameOverTextureOrigin;

        // Fonts
        private SpriteFont font;

        // Messages
        private readonly StringBuilder S_Reset = new();
        private readonly StringBuilder S_Time = new();
        private readonly StringBuilder S_Level = new();

        private Vector2 S_ResetMeasured;
        private Vector2 S_TimeMeasured;
        private Vector2 S_LevelMeasured;

        protected override void OnInitialize()
        {
            this.backgroundTexture = STextures.GetTexture("UI_SolidBackground");
            this.gameOverTexture = STextures.GetTexture("TEXTS_GameOver");

            this.backgroundTextureOrigin = this.backgroundTexture.GetOriginPosition();
            this.gameOverTextureOrigin = this.gameOverTexture.GetOriginPosition();

            this.font = SFonts.Impact;

            _ = this.S_Reset.Append("Press R Key to Reset!");
            this.S_ResetMeasured = this.font.MeasureString(this.S_Reset);
        }
        protected override void OnUpdate()
        {
            if (SInput.Started(Keys.R))
            {
                SGameController.Reset();
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(this.backgroundTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, new Color(1, 11, 25, 180), 0f, this.backgroundTextureOrigin, new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.Draw(this.gameOverTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y - 96), null, Color.White, 0f, this.gameOverTextureOrigin, new Vector2(0.5f), SpriteEffects.None, 0f);

            SGraphics.SpriteBatch.DrawString(this.font, this.S_Level, new Vector2(SCamera.Center.X - (this.S_LevelMeasured.X / 2), SCamera.Center.Y), Color.White, 0f, Vector2.Zero, new Vector2(1f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Time, new Vector2(SCamera.Center.X - (this.S_TimeMeasured.X / 2), SCamera.Center.Y + 16), Color.White, 0f, Vector2.Zero, new Vector2(1f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Reset, new Vector2(SCamera.Center.X - (this.S_ResetMeasured.X / 2), SCamera.Center.Y + 93), Color.White, 0f, Vector2.Zero, new Vector2(1f), SpriteEffects.None, 0f);
        }

        internal void Build(TimeSpan time, int level)
        {
            _ = this.S_Time.Clear();
            _ = this.S_Level.Clear();

            _ = this.S_Time.Append($"Time: {time.Hours}:{time.Minutes}:{time.Seconds}:{time.Milliseconds}");
            _ = this.S_Level.Append($"Level: {level}");

            this.S_TimeMeasured = this.font.MeasureString(this.S_Time);
            this.S_LevelMeasured = this.font.MeasureString(this.S_Level);
        }
    }
}