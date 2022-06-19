using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public struct MusicData
    {
        public string key;
        public AudioClip value;
        public float volume;
        public bool loop;
        public bool persistent;
    }
}