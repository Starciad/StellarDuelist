using Microsoft.Xna.Framework.Media;

using System.Collections.Generic;

namespace StardustDefender.Core.Components
{
    public static class SSongs
    {
        public static float Volume { get; set; } = 0.5f;

        private static Song currentSong;

        private static readonly Dictionary<string, Song> songs = new();
        private static readonly (string, string)[] assets = new (string, string)[]
        {
            // Area
            ("Area_1", "KirbySuperStar/Area_1"),
            ("Area_2", "KirbySuperStar/Area_2"),
            ("Area_3", "KirbySuperStar/Area_3"),
            ("Area_4", "KirbySuperStar/Area_4"),
            ("Area_5", "KirbySuperStar/Area_5"),
            ("Area_6", "KirbySuperStar/Area_6"),
            ("Area_7", "KirbySuperStar/Area_7"),
            ("Area_8", "KirbySuperStar/Area_8"),
            ("Area_9", "KirbySuperStar/Area_9"),
            ("Area_10", "KirbySuperStar/Area_10"),
            ("Area_11", "KirbySuperStar/Area_11"),
            ("Area_12", "KirbySuperStar/Area_12"),
            ("Area_13", "KirbySuperStar/Area_13"),
            ("Area_14", "KirbySuperStar/Area_14"),
            ("Area_15", "KirbySuperStar/Area_15"),
            ("Area_16", "KirbySuperStar/Area_16"),

            // Boss (Incoming)
            ("Boss_Incoming_1", "KirbySuperStar/Boss_Incoming_1"),
            ("Boss_Incoming_2", "KirbySuperStar/Boss_Incoming_2"),
            ("Boss_Incoming_3", "KirbySuperStar/Boss_Incoming_3"),
            ("Boss_Incoming_4", "KirbySuperStar/Boss_Incoming_4"),
            ("Boss_Incoming_5", "KirbySuperStar/Boss_Incoming_5"),

            // Boss
            ("Boss_1", "KirbySuperStar/Boss_1"),
            ("Boss_2", "KirbySuperStar/Boss_2"),
            ("Boss_3", "KirbySuperStar/Boss_3"),
            ("Boss_4", "KirbySuperStar/Boss_4"),
            ("Boss_5", "KirbySuperStar/Boss_5"),

            // Game Over
            ("Game_Over_1", "KirbySuperStar/Game_Over_1"),
            ("Game_Over_2", "KirbySuperStar/Game_Over_2"),
            ("Game_Over_3", "KirbySuperStar/Game_Over_3"),

            // Victory
            ("Victory_1", "KirbySuperStar/Victory_1"),
            ("Victory_2", "KirbySuperStar/Victory_2"),
            ("Victory_3", "KirbySuperStar/Victory_3"),
            ("Victory_Boss", "KirbySuperStar/Victory_Boss"),

            // Opening
            ("Opening_1", "KirbySuperStar/Opening_1"),
            ("Opening_2", "KirbySuperStar/Opening_2"),
            ("Opening_3", "KirbySuperStar/Opening_3"),
            ("Opening_4", "KirbySuperStar/Opening_4"),
            ("Opening_5", "KirbySuperStar/Opening_5"),
        };

        internal static void Load()
        {
            MediaPlayer.Volume = Volume;
            MediaPlayer.IsRepeating = true;

            foreach ((string, string) asset in assets)
            {
                songs.Add(asset.Item1, SContent.Sprites.Load<Song>(asset.Item2));
            }
        }

        public static void Play(string name)
        {
            if (currentSong != null)
            {
                Stop();
            }

            Song song = songs[name];

            MediaPlayer.Play(song);
            currentSong = song;
        }
        public static void Mute()
        {
            MediaPlayer.IsMuted = true;
        }
        public static void Unmute()
        {
            MediaPlayer.IsMuted = false;
        }
        public static void Pause()
        {
            MediaPlayer.Pause();
        }
        public static void Stop()
        {
            MediaPlayer.Stop();
        }
        public static void Resume()
        {
            MediaPlayer.Resume();
        }
    }
}
