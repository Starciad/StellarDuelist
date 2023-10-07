using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;

namespace StardustDefender.Core.Components
{
    public static class SSongs
    {
        public static float Volume { get; set; } = 0.5f;

        private static Song currentSong;

        private static readonly Dictionary<string, Song> songs = new();
        private static readonly (string, string)[] assets = Array.Empty<(string, string)>();

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
