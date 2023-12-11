using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardustDefender.Core.Camera;
using StardustDefender.Core.Components;
using StardustDefender.Core.Controllers;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;

using System.Reflection;
using System;

#if PC
using StardustDefender.CaptureSystem;
#endif

namespace StardustDefender.Core
{
    /// <summary>
    /// Responsible for containing the main components for the standardized and complete initialization of StardustDefender.
    /// </summary>
    /// <remarks>
    /// Derived from the <see cref="Game"/> class, <see cref="SGame"/> is an abstraction that enables better automation of various trivial processes during the game's initialization, such as creating and preparing managers and controllers, registering entities, items, and more, among other important functions.
    /// </remarks>
    public sealed class SGame : Game
    {
        /// <summary>
        /// Reference to the Assembly in which <see cref="SGame"/> was originally created.
        /// </summary>
        internal static Assembly Assembly { get; private set; }

        /// <summary>
        /// Static instance for quick access to information about the currently running <see cref="SGame"/>.
        /// </summary>
        internal static SGame Instance { get; private set; }

        /// <summary>
        /// Creates and initializes the fundamental processes for the game's execution.
        /// </summary>
        /// <param name="assembly">The Assembly where the current <see cref="SGame"/> is being created.</param>
        public SGame(Assembly assembly)
        {
            // Initialize the game's graphics.
            SGraphics.Build(new(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = SScreen.Width,
                PreferredBackBufferHeight = SScreen.Height,
                SynchronizeWithVerticalRetrace = false,
            });

            // Initialize Content
            SContent.Build(this.Content.ServiceProvider, "Content");

            // Set the Assembly reference
            Assembly = assembly;

            // Configure the game's window
            this.Window.Title = SInfos.GetTitle();
            this.Window.AllowUserResizing = false;
            this.Window.IsBorderless = false;

            // Configure game settings
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = SGraphics.FPS;

            // Set the instance to this object
            Instance = this;
        }

        // ============================================================= //
        // All the methods below are called by the MonoGame framework    //
        // during various stages of processing. They are ordered by the  //
        // order of execution.                                           //
        // ============================================================= //

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            SSongs.Resume();
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            SSongs.Pause();
        }

        #region GAME ROUTINE
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

            base.LoadContent();
        }

        protected override void BeginRun()
        {
            // Initialize various managers.
            SEntityManager.Initialize();
            SGUIManager.Initialize();
            SEffectsManager.Initialize();
            SItemsManager.Initialize();

            // Initialize controllers.
            SLevelController.Initialize();
            SDifficultyController.Initialize();

            // BeginRun for certain controllers.
            SGameController.BeginRun();
            SDifficultyController.BeginRun();
            SBackgroundController.BeginRun();

            // Start the game level.
            SLevelController.BeginRun();

            base.BeginRun();
        }

        protected override void Update(GameTime gameTime)
        {
#if PC
            // Screenshot
            if (SInput.Started(Keys.F12))
            {
                SScreenshot.Print(SGraphics.DefaultRenderTarget);
            }
#endif

            // Update time and input.
            STime.SetUpdateGameTime(value: gameTime);
            SInput.Update();

            if (this.IsActive && SGameController.State == SGameState.Running)
            {
                // Update various managers.
                SItemsManager.Update();
                SEffectsManager.Update();
                SProjectileManager.Update();
                SEntityManager.Update();

                // Update game controllers.
                SLevelController.Update();
            }

            if (this.IsActive && SGameController.State != SGameState.Paused)
            {
                // Update the game's background.
                SBackgroundController.Update();
            }

            // Update fading effects and GUI.
            SFade.Update();
            SGUIManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            STime.SetDrawGameTime(value: gameTime);

            // Set the render target for rendering game elements.
            this.GraphicsDevice.SetRenderTarget(SGraphics.DefaultRenderTarget);
            this.GraphicsDevice.Clear(new Color(1, 11, 25));

            SGraphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, SCamera.GetViewMatrix());

            // Draw various game elements.
            SBackgroundController.Draw();
            SEntityManager.Draw();
            SProjectileManager.Draw();
            SEffectsManager.Draw();
            SItemsManager.Draw();
            SGUIManager.Draw();
            SFade.Draw();

            SGraphics.SpriteBatch.End();

            // Set the render target back to null and render to the screen.
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
        #endregion
    }
}
