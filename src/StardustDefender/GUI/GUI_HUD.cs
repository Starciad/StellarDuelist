using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Colors;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Extensions;
using StardustDefender.Core.GUI;

using System.Text;

namespace StardustDefender.GUI
{
    internal sealed class GUI_HUD : SGUI
    {
        // Textures
        private Texture2D borderTexture;
        private Texture2D logoTexture;
        private Texture2D tutorialTexture;
        private Texture2D madeByStarciadTexture;

        // Origins
        private Vector2 borderTextureOrigin;
        private Vector2 logoTextureOrigin;
        private Vector2 tutorialTextureOrigin;
        private Vector2 madeByStarciadTextureOrigin;

        // Fonts
        private SpriteFont font;

        // INFOS (Left)
        private readonly StringBuilder S_Boss_Incoming = new("BOSS INCOMING");
        private readonly StringBuilder S_Difficulty = new();
        private readonly StringBuilder S_Level = new();
        private readonly StringBuilder S_Health = new();
        private readonly StringBuilder S_Damage = new();
        private readonly StringBuilder S_BulletSpeed = new();
        private readonly StringBuilder S_BulletDelay = new();
        private readonly StringBuilder S_BulletLife = new();

        // INFOS (Right)
        private readonly StringBuilder S_TotalGameTime_Label = new("GAME TIME");
        private readonly StringBuilder S_TotalGameTime = new();

        // Properties
        private bool viewedTheTutorial = false;
        private byte tutorialTimeout;
        private int warningPalleteColorIndex;

        protected override bool ConditionToBeDrawn()
        {
            return SGameController.State == SGameState.Running;
        }

        protected override void OnInitialize()
        {
            // Textures
            this.borderTexture = STextures.GetTexture("UI_SolidBackground");
            this.logoTexture = STextures.GetTexture("UI_Logo");
            this.tutorialTexture = STextures.GetTexture("TEXTS_Tutorial");
            this.madeByStarciadTexture = STextures.GetTexture("TEXTS_MadeByStarciad");

            // Origins
            this.borderTextureOrigin = this.borderTexture.GetOriginPosition();
            this.logoTextureOrigin = this.logoTexture.GetOriginPosition();
            this.tutorialTextureOrigin = this.tutorialTexture.GetOriginPosition();
            this.madeByStarciadTextureOrigin = this.madeByStarciadTexture.GetOriginPosition();

            // Fonts
            this.font = SFonts.Impact;
        }
        protected override void OnUpdate()
        {
            UpdateColors();

            _ = this.S_Difficulty.Clear();
            _ = this.S_Level.Clear();
            _ = this.S_Health.Clear();
            _ = this.S_Damage.Clear();
            _ = this.S_BulletSpeed.Clear();
            _ = this.S_BulletDelay.Clear();
            _ = this.S_BulletLife.Clear();
            _ = this.S_TotalGameTime.Clear();

            _ = this.S_Difficulty.Append($"{SDifficultyController.GetDifficultyLabel()}");
            _ = this.S_Level.Append($"Level: {SLevelController.Level + 1}");
            _ = this.S_Health.Append($"Health: {SLevelController.Player.HealthValue}");
            _ = this.S_Damage.Append($"Damage: {SLevelController.Player.DamageValue}");
            _ = this.S_BulletSpeed.Append($"Bullet Speed: {SLevelController.Player.BulletSpeed.ToString("#.0")}");
            _ = this.S_BulletDelay.Append($"Shoot Delay: {SLevelController.Player.ShootDelay.ToString("#.0")}");
            _ = this.S_BulletLife.Append($"Bullet Life: {SLevelController.Player.BulletLifeTime.ToString("#.0")}");
            _ = this.S_TotalGameTime.Append($"{SLevelController.TotalGameTime.ToString(@"hh\:mm\:ss")}");

            if (!this.viewedTheTutorial)
            {
                if (SInput.Started(Keys.A) || SInput.Started(Keys.D) || SInput.Started(Keys.K) ||
                    SInput.Started(Keys.Left) || SInput.Started(Keys.Right) || SInput.Started(Keys.P))
                {
                    this.tutorialTimeout++;
                }

                if (this.tutorialTimeout >= 6)
                {
                    this.viewedTheTutorial = true;
                }
            }
        }
        protected override void OnDraw()
        {
            // Borders
            SGraphics.SpriteBatch.Draw(this.borderTexture, new Vector2(SCamera.Center.X - 800, SCamera.Center.Y), null, new Color(0, 0, 0, 170), 0f, this.borderTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.Draw(this.borderTexture, new Vector2(SCamera.Center.X + 800, SCamera.Center.Y), null, new Color(0, 0, 0, 170), 0f, this.borderTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);

            // Logo
            SGraphics.SpriteBatch.Draw(this.logoTexture, new Vector2(SCamera.Center.X - 208, SCamera.Center.Y - 112), null, Color.White, 0f, this.logoTextureOrigin, new Vector2(0.5f), SpriteEffects.None, 0f);

            // Infos (Left)
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Difficulty, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 29), Color.MonoGameOrange, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Level, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 45), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Health, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 61), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_Damage, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 77), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_BulletDelay, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 93), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_BulletSpeed, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 109), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_BulletLife, new Vector2(SCamera.Center.X - 250, SCamera.Center.Y + 125), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);

            // Infos (Right)
            // TOP
            SGraphics.SpriteBatch.Draw(this.madeByStarciadTexture, new Vector2(SCamera.Center.X + 208, SCamera.Center.Y - 112), null, Color.White, 0f, this.madeByStarciadTextureOrigin, new Vector2(0.3f), SpriteEffects.None, 0f);

            // BOTTOM
            SGraphics.SpriteBatch.DrawString(this.font, this.S_TotalGameTime_Label, new Vector2(SCamera.Center.X + 180, SCamera.Center.Y + 109), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(this.font, this.S_TotalGameTime, new Vector2(SCamera.Center.X + 186, SCamera.Center.Y + 125), Color.White, 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);

            if (SLevelController.BossIncoming && !SLevelController.BossAppeared)
            {
                SGraphics.SpriteBatch.DrawString(this.font, this.S_Boss_Incoming, new Vector2(SCamera.Center.X + 168, SCamera.Center.Y + 90), Palettes.WARNING_PALETTE[this.warningPalleteColorIndex], 0f, Vector2.Zero, new Vector2(0.3f), SpriteEffects.None, 0f);
            }

            // Tutorial
            if (!this.viewedTheTutorial)
            {
                SGraphics.SpriteBatch.Draw(this.tutorialTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, Color.White, 0f, this.tutorialTextureOrigin, new Vector2(0.75f), SpriteEffects.None, 0f);
            }
        }

        private void UpdateColors()
        {
            this.warningPalleteColorIndex = this.warningPalleteColorIndex < Palettes.WARNING_PALETTE.Length - 1 ? this.warningPalleteColorIndex + 1 : 0;
        }
    }
}
