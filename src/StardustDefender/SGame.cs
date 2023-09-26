using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Camera;
using StardustDefender.Controllers;
using StardustDefender.Core;
using StardustDefender.Enums;
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
            SContent.Build(this.Content.ServiceProvider, "Content");

            // Assembly
            Assembly = GetType().Assembly;

            // Window
            this.Window.Title = SInfos.GetTitle();
            this.Window.AllowUserResizing = false;
            this.Window.IsBorderless = false;

            // Settings
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
        }

        protected override void Initialize()
        {
            // Engine
            SGraphics.Initialize();
            SScreen.Initialize(SGraphics.GraphicsDevice.Viewport);
            SCamera.Initialize();

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
            SLevelController.Initialize();
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

            // Game
            if (this.IsActive && SGameController.State == SGameState.Running)
            {
                // Managers
                SItemsManager.Update();
                SEffectsManager.Update();
                SProjectileManager.Update();
                SEntityManager.Update();

                // Controllers
                SLevelController.Update();
            }

            if (this.IsActive && SGameController.State != SGameState.Paused)
            {
                SBackgroundController.Update();
            }

            // Engine
            SFade.Update();
            SGUIManager.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            STime.Update(null, gameTime);

            // ========================= //
            // Targets

            this.GraphicsDevice.SetRenderTarget(SGraphics.DefaultRenderTarget);
            this.GraphicsDevice.Clear(new Color(1, 11, 25));

            SGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, SCamera.GetViewMatrix());
            DrawGameElements();
            DrawGameGUI();
            SFade.Draw();
            SGraphics.SpriteBatch.End();

            // ========================= //
            // Content

            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);

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
            // INTRODUCTION
            if (SGameController.State == SGameState.Introduction)
            {
                SGUIManager.Enable<SGUIIntroduction>();
            }
            else
            {
                SGUIManager.Disable<SGUIIntroduction>();
            }

            // RUNNING
            if (SGameController.State == SGameState.Running)
            {
                SGUIManager.Enable<SGUI_HUD>();
            }
            else
            {
                SGUIManager.Disable<SGUI_HUD>();
            }

            // PAUSE
            if (SGameController.State == SGameState.Paused)
            {
                SGUIManager.Enable<SGUIPause>();
            }
            else
            {
                SGUIManager.Disable<SGUIPause>();
            }

            // GAME OVER
            if (SGameController.State == SGameState.GameOver)
            {
                SGUIManager.Enable<SGUIGameOver>();
            }
            else
            {
                SGUIManager.Disable<SGUIGameOver>();
            }

            SGUIManager.Draw();
        }
    }
}