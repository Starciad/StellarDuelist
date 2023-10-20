using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.IO;

namespace StardustDefender.Core.Components
{
    /// <summary>
    /// Static utility for accessing a collection of content managers.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This utility houses a series of content managers that access specific directories and locations based on their designated function. It assists in the organization and logical loading of content in the game while serving as a shortcut to finding particular assets.
    ///     </para>
    /// </remarks>
    internal static class SContent
    {
        /// <summary>
        /// Content manager designated for game sprites.
        /// </summary>
        internal static ContentManager Sprites => _contentManagers["Sprites"];

        /// <summary>
        /// Content manager designated for game music.
        /// </summary>
        internal static ContentManager Sounds => _contentManagers["Sounds"];

        /// <summary>
        /// Content manager designated for game songs.
        /// </summary>
        internal static ContentManager Songs => _contentManagers["Songs"];

        /// <summary>
        /// Content manager designated for game fonts.
        /// </summary>
        internal static ContentManager Fonts => _contentManagers["Fonts"];

        private static readonly Dictionary<string, ContentManager> _contentManagers = new();
        private static readonly string[] namesOfContentManagers = new string[]
        {
            "Sprites",
            "Sounds",
            "Songs",
            "Fonts",
        };

        /// <summary>
        /// Creates, builds, and initializes all predefined content managers, configuring their paths and the specific assets they will manage.
        /// </summary>
        /// <param name="serviceProvider">The main service provider to be assigned to the content managers.</param>
        /// <param name="relativePath">Relative path to the project's Content/Assets folder.</param>
        internal static void Build(IServiceProvider serviceProvider, string relativePath)
        {
            foreach (string name in namesOfContentManagers)
            {
                _contentManagers.Add(name, new(serviceProvider, Path.Combine(relativePath, name)));
            }
        }
    }
}
