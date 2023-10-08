using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace StardustDefender.Core.Components
{
    public static class STextures
    {
        private static readonly Dictionary<string, Texture2D> textures = new();
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

            // ITEMS
            ("ITEMS_Upgrades", "Items/Upgrades"),
        };

        internal static void Load()
        {
            foreach ((string, string) asset in assets)
            {
                textures.Add(asset.Item1, SContent.Sprites.Load<Texture2D>(asset.Item2));
            }
        }

        public static Texture2D GetTexture(string id)
        {
            return textures[id];
        }
        public static Rectangle GetSprite(int scale, int pivotX, int pivotY)
        {
            return new(new(pivotX * scale, pivotY * scale), new(scale));
            ;
        }
        public static Rectangle GetSprite(int scaleX, int scaleY, int pivotX, int pivotY)
        {
            return new(new(pivotX * scaleX, pivotY * scaleY), new(scaleX, scaleY));
            ;
        }
    }
}