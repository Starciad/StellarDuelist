using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace StardustDefender.Engine
{
    internal static class SSounds
    {
        internal static float Volume { get; set; } = 0.5f;
        internal static float Pitch { get; set; } = 1f;

        private static readonly Dictionary<string, SoundEffect> soundEffects = new();

        internal static void Load()
        {
            SoundEffect.MasterVolume = Volume;

            // Inputs
            soundEffects.Add("Player_Movement", SContent.Sounds.Load<SoundEffect>("Input/player_movement"));
            soundEffects.Add("Player_Upgrade", SContent.Sounds.Load<SoundEffect>("Input/player_upgrade"));

            // Explosion
            soundEffects.Add("Explosion_01", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_01"));
            soundEffects.Add("Explosion_02", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_02"));
            soundEffects.Add("Explosion_03", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_03"));
            soundEffects.Add("Explosion_04", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_04"));
            soundEffects.Add("Explosion_05", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_05"));
            soundEffects.Add("Explosion_06", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_06"));
            soundEffects.Add("Explosion_07", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_07"));
            soundEffects.Add("Explosion_08", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_08"));
            soundEffects.Add("Explosion_09", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_09"));
            soundEffects.Add("Explosion_10", SContent.Sounds.Load<SoundEffect>("Explosion/Explosion_10"));

            // Damage
            soundEffects.Add("Damage_01", SContent.Sounds.Load<SoundEffect>("Damage/Damage_01"));
            soundEffects.Add("Damage_02", SContent.Sounds.Load<SoundEffect>("Damage/Damage_02"));
            soundEffects.Add("Damage_03", SContent.Sounds.Load<SoundEffect>("Damage/Damage_03"));
            soundEffects.Add("Damage_04", SContent.Sounds.Load<SoundEffect>("Damage/Damage_04"));
            soundEffects.Add("Damage_05", SContent.Sounds.Load<SoundEffect>("Damage/Damage_05"));
            soundEffects.Add("Damage_06", SContent.Sounds.Load<SoundEffect>("Damage/Damage_06"));
            soundEffects.Add("Damage_07", SContent.Sounds.Load<SoundEffect>("Damage/Damage_07"));
            soundEffects.Add("Damage_08", SContent.Sounds.Load<SoundEffect>("Damage/Damage_08"));
            soundEffects.Add("Damage_09", SContent.Sounds.Load<SoundEffect>("Damage/Damage_09"));
            soundEffects.Add("Damage_10", SContent.Sounds.Load<SoundEffect>("Damage/Damage_10"));

            // Shoots
            soundEffects.Add("Shoot_01", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_01"));
            soundEffects.Add("Shoot_02", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_02"));
            soundEffects.Add("Shoot_03", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_03"));
            soundEffects.Add("Shoot_04", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_04"));
            soundEffects.Add("Shoot_05", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_05"));
            soundEffects.Add("Shoot_06", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_06"));
            soundEffects.Add("Shoot_07", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_07"));
            soundEffects.Add("Shoot_08", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_08"));
            soundEffects.Add("Shoot_09", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_09"));
            soundEffects.Add("Shoot_10", SContent.Sounds.Load<SoundEffect>("Shoot/Shoot_10"));
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
