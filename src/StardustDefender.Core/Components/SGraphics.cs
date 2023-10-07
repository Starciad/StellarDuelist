using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace StardustDefender.Core.Components
{
    public static class SGraphics
    {
        public static GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public static GraphicsDevice GraphicsDevice => _graphicsDeviceManager.GraphicsDevice;
        public static RenderTarget2D DefaultRenderTarget => _defaultRenderTarget;
        public static SpriteBatch SpriteBatch => _spriteBatch;
        public static TimeSpan FPS => TimeSpan.FromSeconds(1f / 60f);

        private static GraphicsDeviceManager _graphicsDeviceManager;
        private static GraphicsDevice _graphicsDevice;
        private static RenderTarget2D _defaultRenderTarget;
        private static SpriteBatch _spriteBatch;

        internal static void Build(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
        }
        internal static void Initialize()
        {
            _graphicsDevice = _graphicsDeviceManager.GraphicsDevice;
            _defaultRenderTarget = new(_graphicsDevice, SScreen.Width, SScreen.Height);
            _spriteBatch = new(GraphicsDevice);
        }
    }
}
