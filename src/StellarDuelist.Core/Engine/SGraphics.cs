using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// Static utility responsible for controlling essential graphics components for managing all graphical information related to the game.
    /// </summary>
    public static class SGraphics
    {
        /// <summary>
        /// Graphics device manager.
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;

        /// <summary>
        /// Graphics device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice => _graphicsDeviceManager.GraphicsDevice;

        /// <summary>
        /// Rendering target that serves as a buffer for the early rendering of all primary game components.
        /// </summary>
        /// <remarks>
        /// Before everything is actually rendered and displayed on the client's screen, all graphic contents are early drawn in an organized manner on a rendering target. This allows the application of various interesting and utilitarian effects and elements.
        /// </remarks>
        public static RenderTarget2D DefaultRenderTarget => _defaultRenderTarget;

        /// <summary>
        /// Default SpriteBatch used throughout the project for rendering graphical elements.
        /// </summary>
        public static SpriteBatch SpriteBatch => _spriteBatch;

        /// <summary>
        /// Default constant FPS defined for game execution.
        /// </summary>
        public static TimeSpan FPS => TimeSpan.FromSeconds(1f / 60f);

        private static GraphicsDeviceManager _graphicsDeviceManager;
        private static GraphicsDevice _graphicsDevice;
        private static RenderTarget2D _defaultRenderTarget;
        private static SpriteBatch _spriteBatch;

        /// <summary>
        /// Initialize and prepare graphics-related information.
        /// </summary>
        /// <param name="graphicsDeviceManager">Graphics device manager used in the current game instance.</param>
        internal static void Build(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
        }

        /// <summary>
        /// Initialize, configure, and prepare the main graphic components used in the current game instance.
        /// </summary>
        internal static void Initialize()
        {
            _graphicsDevice = _graphicsDeviceManager.GraphicsDevice;
            _defaultRenderTarget = new(_graphicsDevice, SScreen.Width, SScreen.Height);
            _spriteBatch = new(GraphicsDevice);
        }
    }
}
