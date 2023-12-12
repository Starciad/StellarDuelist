using StellarDuelist.Core.Background;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Controllers
{
    /// <summary>
    /// A static class responsible for managing background elements.
    /// </summary>
    internal static class SBackgroundController
    {
        /// <summary>
        /// Gets or sets the global parallax factor that affects all background elements.
        /// </summary>
        internal static float GlobalParallaxFactor { get; set; }

        private static readonly List<SBackground> backgrounds = new();

        /// <summary>
        /// Initializes the background controller and loads background elements from the game assembly.
        /// </summary>
        internal static void BeginRun()
        {
            Reset();

            foreach (Type type in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SBackground))))
            {
                SBackground background = (SBackground)Activator.CreateInstance(type);
                background.Initialize();

                backgrounds.Add(background);
            }
        }

        /// <summary>
        /// Updates all background elements.
        /// </summary>
        internal static void Update()
        {
            backgrounds.ForEach(x => x.Update());
        }

        /// <summary>
        /// Render all background elements with their parallax effects.
        /// </summary>
        internal static void Draw()
        {
            backgrounds.ForEach(x => x.Draw());
        }

        /// <summary>
        /// Resets the background controller.
        /// </summary>
        internal static void Reset()
        {
            GlobalParallaxFactor = 1f;
        }
    }
}
