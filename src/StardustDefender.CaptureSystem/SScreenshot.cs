using Microsoft.Xna.Framework.Graphics;

using System.IO;
using System.Threading.Tasks;

namespace StardustDefender.CaptureSystem
{
    public static class SScreenshot
    {
        private static string ScreenshotsDirectory => Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");

        public static string Print(RenderTarget2D target)
        {
            if (!Directory.Exists(ScreenshotsDirectory))
            {
                _ = Directory.CreateDirectory(ScreenshotsDirectory);
            }

            string filename = Path.Combine(ScreenshotsDirectory, $"Screenshot_{Directory.GetFiles(ScreenshotsDirectory).Length + 1}.png");

            _ = Task.Run(() =>
            {
                using FileStream screenshotFile = File.Create(filename);
                target.SaveAsPng(screenshotFile, target.Width, target.Height);
            });

            return filename;
        }
    }
}
