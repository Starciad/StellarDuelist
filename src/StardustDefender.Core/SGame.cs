using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Controllers;
using StardustDefender.Core.Camera;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

using System.Reflection;

namespace StardustDefender.Core
{
    public sealed class SGame : Game
    {
        internal static Assembly Assembly { get; private set; }
        internal static SGame Instance { get; private set; }

        public SGame(Assembly assembly)
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
            Assembly = assembly;

            // Window
            this.Window.Title = SInfos.GetTitle();
            this.Window.AllowUserResizing = false;
            this.Window.IsBorderless = false;

            // Settings
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = SGraphics.FPS;

            // Finally
            Instance = this;
        }

        protected override void Initialize()
        {
            SGraphics.Initialize();
            SScreen.Initialize(SGraphics.GraphicsDevice.Viewport);
            SCamera.Initialize();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            STextures.Load();
            SSongs.Load();
            SSounds.Load();
            SFonts.Load();
            SFade.Load();
        }
        protected override void BeginRun()
        {
            // Managers
            SEntityManager.Initialize();
            SGUIManager.Initialize();
            SEffectsManager.Initialize();
            SItemsManager.Initialize();

            // Controllers (Initialize)
            SLevelController.Initialize();
            SDifficultyController.Initialize();

            // Controllers (BeginRun)
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
            SBackgroundController.Draw();
            SEntityManager.Draw();
            SProjectileManager.Draw();
            SEffectsManager.Draw();
            SItemsManager.Draw();
            SGUIManager.Draw();
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
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}