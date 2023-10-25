using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace StardustDefender.Core.Components
{
    /// <summary>
    /// Static utility class for managing game sound effects and audio settings.
    /// </summary>
    public static class SSounds
    {
        /// <summary>
        /// Gets or sets the volume of all game sound effects.
        /// </summary>
        public static float Volume { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the pitch for playing sound effects.
        /// </summary>
        public static float Pitch { get; set; } = 1f;

        private static readonly Dictionary<string, SoundEffect> soundEffects = new();
        private static readonly (string, string)[] assets = new (string, string)[]
        {
            // Inputs
            ("Player_Movement", "Input/Player_Movement"),
            ("Player_Upgrade", "Input/Player_Upgrade"),

            // Explosion
            ("Explosion_01", "Explosion/Explosion_01"),
            ("Explosion_02", "Explosion/Explosion_02"),
            ("Explosion_03", "Explosion/Explosion_03"),
            ("Explosion_04", "Explosion/Explosion_04"),
            ("Explosion_05", "Explosion/Explosion_05"),
            ("Explosion_06", "Explosion/Explosion_06"),
            ("Explosion_07", "Explosion/Explosion_07"),
            ("Explosion_08", "Explosion/Explosion_08"),
            ("Explosion_09", "Explosion/Explosion_09"),
            ("Explosion_10", "Explosion/Explosion_10"),

            // Damage
            ("Damage_01", "Damage/Damage_01"),
            ("Damage_02", "Damage/Damage_02"),
            ("Damage_03", "Damage/Damage_03"),
            ("Damage_04", "Damage/Damage_04"),
            ("Damage_05", "Damage/Damage_05"),
            ("Damage_06", "Damage/Damage_06"),
            ("Damage_07", "Damage/Damage_07"),
            ("Damage_08", "Damage/Damage_08"),
            ("Damage_09", "Damage/Damage_09"),
            ("Damage_10", "Damage/Damage_10"),

            // Shoots
            ("Shoot_01", "Shoot/Shoot_01"),
            ("Shoot_02", "Shoot/Shoot_02"),
            ("Shoot_03", "Shoot/Shoot_03"),
            ("Shoot_04", "Shoot/Shoot_04"),
            ("Shoot_05", "Shoot/Shoot_05"),
            ("Shoot_06", "Shoot/Shoot_06"),
            ("Shoot_07", "Shoot/Shoot_07"),
            ("Shoot_08", "Shoot/Shoot_08"),
            ("Shoot_09", "Shoot/Shoot_09"),
            ("Shoot_10", "Shoot/Shoot_10"),
        };

        /// <summary>
        /// Initializes the SSounds class by setting the master volume for all sound effects and loading all sound effect assets.
        /// </summary>
        internal static void Load()
        {
            SoundEffect.MasterVolume = Volume;

            foreach ((string, string) asset in assets)
            {
                soundEffects.Add(asset.Item1, SContent.Sounds.Load<SoundEffect>(asset.Item2));
            }
        }

        /// <summary>
        /// Plays a sound effect by name and returns a SoundEffectInstance.
        /// </summary>
        /// <param name="name">The name of the sound effect to play.</param>
        /// <returns>A SoundEffectInstance for controlling and managing the played sound effect.</returns>
        public static SoundEffectInstance Play(string name)
        {
            SoundEffectInstance instance = soundEffects[name].CreateInstance();

            instance.Pitch = Pitch;
            instance.Play();

            return instance;
        }
    }
}
