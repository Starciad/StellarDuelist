using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Threading.Tasks;

namespace StardustDefender.CaptureSystem
{
    /// <summary>
    /// A utility class for capturing and saving screenshots.
    /// </summary>
    public static class SScreenshot
    {
        private static string ScreenshotsDirectory => Path.Combine(Path.GetFullPath("."), "Screenshots");

        /// <summary>
        /// Capture and save a screenshot from a RenderTarget2D.
        /// </summary> 
        /// <param name="target">The RenderTarget2D to capture the screenshot from.</param>
        /// <returns>The filename of the saved screenshot.</returns>
        public static string Print(RenderTarget2D target)
        {
            if (!Directory.Exists(ScreenshotsDirectory))
            {
                _ = Directory.CreateDirectory(ScreenshotsDirectory);
            }

            string filename = Path.Combine(ScreenshotsDirectory, $"Screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{Guid.NewGuid().ToString()[..5]}.png");

            using (FileStream screenshotFile = File.Create(filename))
            {
                target.SaveAsPng(screenshotFile, target.Width, target.Height);
            }

            return filename;
        }
    }
}
