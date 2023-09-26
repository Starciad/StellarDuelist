using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;

namespace StardustDefender.Core
{
    internal static class SSongs
    {
        internal static float Volume { get; set; } = 0.5f;

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

        internal static void Play(string name)
        {
            if (currentSong != null)
            {
                Stop();
            }

            Song song = songs[name];

            MediaPlayer.Play(song);
            currentSong = song;
        }
        internal static void Mute()
        {
            MediaPlayer.IsMuted = true;
        }
        internal static void Unmute()
        {
            MediaPlayer.IsMuted = false;
        }
        internal static void Pause()
        {
            MediaPlayer.Pause();
        }
        internal static void Stop()
        {
            MediaPlayer.Stop();
        }
        internal static void Resume()
        {
            MediaPlayer.Resume();
        }
    }
}
