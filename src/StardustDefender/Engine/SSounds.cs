using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace StardustDefender.Engine
{
    internal static class SSounds
    {
        internal static float Volume { get; set; } = 0.5f;
        internal static float Pitch { get; set; } = 1f;

        private static readonly Dictionary<string, SoundEffect> soundEffects = new();
        private static readonly (string, string)[] assets = new (string, string)[]
        {
            // Inputs
            ("Player_Movement","Input/player_movement"),
            ("Player_Upgrade", "Input/player_upgrade"),

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

        internal static void Load()
        {
            SoundEffect.MasterVolume = Volume;

            foreach ((string, string) asset in assets)
            {
                soundEffects.Add(asset.Item1, SContent.Sounds.Load<SoundEffect>(asset.Item2));
            }
        }
        internal static SoundEffectInstance Play(string name)
        {
            SoundEffectInstance instance = soundEffects[name].CreateInstance();

            instance.Pitch = Pitch;
            instance.Play();

            return instance;
        }
    }
}
