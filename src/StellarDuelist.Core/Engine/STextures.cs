using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StellarDuelist.Core.Engine
{
    /// <summary>
    /// Static utility class for managing textures and sprite assets in the game.
    /// </summary>
    public static class STextures
    {
        private static readonly Dictionary<string, Texture2D> textures = new();

        // ====================== //
        // All sprites in the list below should have a prefix specifying their category.
        // ====================== //
        private static readonly (string, string)[] assets = new (string, string)[]
        {
            // PLAYER
            ("PLAYER_Spaceship", "Player/Spaceship"),

            // ENEMIES
            ("ENEMIES_Aliens", "Enemies/Aliens"),
            ("ENEMIES_Bosses", "Enemies/Bosses"),

            // PROJECTILES
            ("PROJECTILES_Bullets", "Projectiles/Bullets"),

            // EFFECTS
            ("EFFECTS_Impact", "Effects/Impact"),
            ("EFFECTS_Explosion", "Effects/Explosion"),

            // BACKGROUND
            ("BACKGROUND_01", "Backgrounds/Background_01"),

            // UI
            ("UI_Logo", "UI/Logo/StardustDefenderLogo"),
            ("UI_SolidBackground", "UI/Backgrounds/SolidBackground"),

            // TEXTS
            ("TEXTS_Paused", "UI/Texts/Paused"),
            ("TEXTS_GameOver", "UI/Texts/GameOver"),
            ("TEXTS_Tutorial", "UI/Texts/Tutorial"),
            ("TEXTS_MadeByStarciad", "UI/Texts/MadeByStarciad"),

            // ITEMS
            ("ITEMS_Upgrades", "Items/Upgrades"),
        };

        /// <summary>
        /// Initializes the STextures class by loading all texture assets.
        /// </summary>
        internal static void Load()
        {
            foreach ((string, string) asset in assets)
            {
                textures.Add(asset.Item1, SContent.Sprites.Load<Texture2D>(asset.Item2));
            }
        }

        /// <summary>
        /// Get a texture by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the texture.</param>
        /// <returns>The Texture2D object associated with the specified identifier.</returns>
        public static Texture2D GetTexture(string id)
        {
            return textures[id];
        }

        /// <summary>
        /// Get a sprite rectangle with a specified scale and pivot point.
        /// </summary>
        /// <param name="scale">The scale of the sprite.</param>
        /// <param name="pivotX">The X-coordinate of the pivot point.</param>
        /// <param name="pivotY">The Y-coordinate of the pivot point.</param>
        /// <returns>A Rectangle representing the sprite with the specified scale and pivot point.</returns>
        public static Rectangle GetSprite(int scale, int pivotX, int pivotY)
        {
            return new Rectangle(new Point(pivotX * scale, pivotY * scale), new Point(scale));
        }

        /// <summary>
        /// Get a sprite rectangle with specified X and Y scales and a pivot point.
        /// </summary>
        /// <param name="scaleX">The X-scale of the sprite.</param>
        /// <param name="scaleY">The Y-scale of the sprite.</param>
        /// <param name="pivotX">The X-coordinate of the pivot point.</param>
        /// <param name="pivotY">The Y-coordinate of the pivot point.</param>
        /// <returns>A Rectangle representing the sprite with specified X and Y scales and a pivot point.</returns>
        public static Rectangle GetSprite(int scaleX, int scaleY, int pivotX, int pivotY)
        {
            return new Rectangle(new Point(pivotX * scaleX, pivotY * scaleY), new Point(scaleX, scaleY));
        }
    }
}