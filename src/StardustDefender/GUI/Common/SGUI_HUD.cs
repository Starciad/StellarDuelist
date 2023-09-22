using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Engine;
using StardustDefender.Extensions;

using System.Text;

namespace StardustDefender.GUI.Common
{
    internal sealed class SGUI_HUD : SGUI
    {
        // Textures
        private Texture2D borderTexture;
        private Texture2D logoTexture;

        // Origins
        private Vector2 borderTextureOrigin;
        private Vector2 logoTextureOrigin;

        // Fonts
        private SpriteFont font;

        // Strings
        private readonly StringBuilder S_Level = new();
        private readonly StringBuilder S_Health = new();
        private readonly StringBuilder S_Damage = new();
        private readonly StringBuilder S_BulletSpeed = new();
        private readonly StringBuilder S_BulletDelay = new();
        private readonly StringBuilder S_BulletLife = new();

        protected override void OnInitialize()
        {
            // Textures
            borderTexture = STextures.GetTexture("UI_SolidBackground");
            logoTexture = STextures.GetTexture("UI_Logo");

            // Origins
            borderTextureOrigin = borderTexture.GetOriginPosition();
            logoTextureOrigin = logoTexture.GetOriginPosition();

            // Fonts
            font = SFonts.Impact;
        }
        protected override void OnUpdate()
        {
            S_Level.Clear();
            S_Health.Clear();
            S_Damage.Clear();
            S_BulletSpeed.Clear();
            S_BulletDelay.Clear();
            S_BulletLife.Clear();

            S_Level.Append($"Level: {SLevelController.Level + 1}");
            S_Health.Append($"Health: {SLevelController.Player.HealthValue}");
            S_Damage.Append($"Damage: {SLevelController.Player.DamageValue}");
            S_BulletSpeed.Append($"Bullet Speed: {SLevelController.Player.BulletSpeed.ToString("#.0")}");
            S_BulletDelay.Append($"Shoot Delay: {SLevelController.Player.ShootDelay.ToString("#.0")}");
            S_BulletLife.Append($"Bullet Life: {SLevelController.Player.BulletLifeTime.ToString("#.0")}");
        }
        protected override void OnDraw()
        {
            // Borders
            SGraphics.SpriteBatch.Draw(borderTexture, new Vector2(SCamera.Center.X - 800, SCamera.Center.Y), null, new Color(0, 0, 0, 170), 0f, borderTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.Draw(borderTexture, new Vector2(SCamera.Center.X + 800, SCamera.Center.Y), null, new Color(0, 0, 0, 170), 0f, borderTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);

            // Logo
            SGraphics.SpriteBatch.Draw(logoTexture, new Vector2(SCamera.Center.X - 208, SCamera.Center.Y - 112), null, Color.White, 0f, logoTextureOrigin, new Vector2(0.5f), SpriteEffects.None, 0f);

            // Infos
            SGraphics.SpriteBatch.DrawString(font, S_Level, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 45), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(font, S_Health, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 61), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(font, S_Damage, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 77), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(font, S_BulletDelay, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 93), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(font, S_BulletSpeed, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 109), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.DrawString(font, S_BulletLife, new Vector2(SCamera.Center.X - 248, SCamera.Center.Y + 125), Color.White, 0f, Vector2.Zero, new Vector2(0.8f), SpriteEffects.None, 0f);
        }
    }
}
