using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System.Collections.Generic;

namespace StardustDefender.Engine
{
    internal static class STextures
    {
        private static readonly Dictionary<string, Texture2D> textures = new();

        internal static void Load()
        {
            // Players
            textures.Add("Player_1", SContent.Sprites.Load<Texture2D>("Player/Spaceship"));

            // Enemies
            textures.Add("Aliens", SContent.Sprites.Load<Texture2D>("Enemies/Aliens"));

            // Projectiles
            textures.Add("Bullets", SContent.Sprites.Load<Texture2D>("Projectiles/Bullets"));

            // Effects
            textures.Add("Effects_Impact", SContent.Sprites.Load<Texture2D>("Effects/Impact"));
            textures.Add("Effects_Explosion", SContent.Sprites.Load<Texture2D>("Effects/Explosion"));

            // Background
            textures.Add("Background_01", SContent.Sprites.Load<Texture2D>("Backgrounds/Background_01"));

            // UI
            textures.Add("UI_Logo", SContent.Sprites.Load<Texture2D>("UI/Logo/StardustDefenderLogo"));
            textures.Add("UI_SolidBackground", SContent.Sprites.Load<Texture2D>("UI/Backgrounds/SolidBackground"));

            // Texts
            textures.Add("UI_Paused", SContent.Sprites.Load<Texture2D>("UI/Texts/Paused"));

            // Items
            textures.Add("Upgrades", SContent.Sprites.Load<Texture2D>("Items/Upgrades"));
        }

        internal static Texture2D GetTexture(string id)
        {
            return textures[id];
        }
        internal static Rectangle GetSprite(int scale, int pivotX, int pivotY)
        {
            return new(new(pivotX * scale, pivotY * scale), new(scale)); ;
        }
        internal static Rectangle GetSprite(int scaleX, int scaleY, int pivotX, int pivotY)
        {
            return new(new(pivotX * scaleX, pivotY * scaleY), new(scaleX, scaleY)); ;
        }
    }
}