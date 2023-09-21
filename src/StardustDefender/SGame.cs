using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Engine;
using StardustDefender.GUI.Common;
using StardustDefender.Managers;

using System;
using System.Reflection;

namespace StardustDefender
{
    internal sealed class SGame : Game
    {
        internal static Assembly Assembly { get; private set; }

        internal SGame()
        {
            SGraphics.Build(new(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = SScreen.Width,
                PreferredBackBufferHeight = SScreen.Height,
                SynchronizeWithVerticalRetrace = false,
            });

            // Content
            SContent.Build(Content.ServiceProvider, "Content");

            // Assembly
            Assembly = GetType().Assembly;

            // Window
            Window.Title = "Stardust Defender - v0.0.1";
            Window.AllowUserResizing = false;
            Window.IsBorderless = false;

            // Settings
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
        }

        protected override void Initialize()
        {
            // Engine
            SGraphics.Initialize();
            SScreen.Initialize(SGraphics.GraphicsDevice.Viewport);
            SCamera.Initialize();

            // Controllers
            SLevelController.Initialize();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            STextures.Load();
            SSounds.Load();
            SFonts.Load();
            SFade.Load();
        }
        protected override void BeginRun()
        {
            // Managers
            SGUIManager.Initialize();
            SEffectsManager.Initialize();
            SItemsManager.Initialize();

            // Controllers
            SGameController.BeginRun();
            SDifficultyController.BeginRun();
            SBackgroundController.BeginRun();

            // Level
            SLevelController.BeginRun();
        }
        protected override void Update(GameTime gameTime)
        {
            STime.Update(gameTime, null);
            SInput.Update();

            // Engine
            SFade.Update();
            SGUIManager.Update();
            SBackgroundController.Update();

            // Game
            if (SGameController.State == SGameState.Running)
            {
                // Managers
                SItemsManager.Update();
                SEffectsManager.Update();
                SProjectileManager.Update();
                SEntityManager.Update();
                
                // Controllers
                SLevelController.Update();
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            STime.Update(null, gameTime);

            // ========================= //
            // Targets

            GraphicsDevice.SetRenderTarget(SGraphics.DefaultRenderTarget);
            GraphicsDevice.Clear(new Color(1, 11, 25));

            SGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, SCamera.GetViewMatrix());
            DrawGameElements();
            DrawGameGUI();
            SFade.Draw();
            SGraphics.SpriteBatch.End();

            // ========================= //
            // Content

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            SGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            SGraphics.SpriteBatch.Draw(SGraphics.DefaultRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private static void DrawGameElements()
        {
            SBackgroundController.Draw();
            SEntityManager.Draw();
            SProjectileManager.Draw();
            SEffectsManager.Draw();
            SItemsManager.Draw();
        }
        private static void DrawGameGUI()
        {
            if (SGameController.State == SGameState.Introduction)
            {
                SGUIManager.DisableAll();
                SGUIManager.Enable<SGUIIntroduction>();
            }

            if (SGameController.State == SGameState.Running)
            {

            }

            if (SGameController.State == SGameState.Paused)
            {

            }

            if (SGameController.State == SGameState.Victory)
            {

            }

            if (SGameController.State == SGameState.GameOver)
            {

            }

            SGUIManager.Draw();
        }
    }
}