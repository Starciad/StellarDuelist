using Microsoft.Xna.Framework.Graphics;

using System.IO;

namespace StardustDefender.CaptureSystem
{
    public static class SScreenshot
    {
        private static string SCREENSHOTS_DIRECTORY => Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");

        public static void Print(RenderTarget2D target)
        {
            if (!Directory.Exists(SCREENSHOTS_DIRECTORY))
            {
                _ = Directory.CreateDirectory(SCREENSHOTS_DIRECTORY);
            }
            
            using FileStream screenshotFile = File.Create(Path.Combine(SCREENSHOTS_DIRECTORY, $"Screenshot_{Directory.GetFiles(SCREENSHOTS_DIRECTORY).Length + 1}.png"));
            target.SaveAsPng(screenshotFile, target.Width, target.Height);
        }
    }
}
