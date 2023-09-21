using Microsoft.Xna.Framework.Content;

using System;
using System.Collections.Generic;
using System.IO;

namespace StardustDefender.Engine
{
    internal static class SContent
    {
        internal static ContentManager Sprites => _contentManagers["Sprites"];
        internal static ContentManager Sounds => _contentManagers["Sounds"];
        internal static ContentManager Songs => _contentManagers["Songs"];
        internal static ContentManager Fonts => _contentManagers["Fonts"];

        private static readonly Dictionary<string, ContentManager> _contentManagers = new();
        private static readonly string[] namesOfContentManagers = new string[]
        {
            "Sprites",
            "Sounds",
            "Songs",
            "Fonts",
        };

        internal static void Build(IServiceProvider serviceProvider, string relativePath)
        {
            foreach (string name in namesOfContentManagers)
            {
                _contentManagers.Add(name, new(serviceProvider, Path.Combine(relativePath, name)));
            }
        }
    }
}
