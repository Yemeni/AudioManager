using System;
using UnityEngine;

namespace CarterGames.Legacy.AudioManager
{
    [Serializable]
    public class ClipData
    {
        [SerializeField] private AudioClip clip;

        public string ClipName => clip != null ? clip.name : "Null";
        public AudioClip Clip => clip;
        
        public ClipData(AudioClip clip)
        {
            this.clip = clip;
        }
    }
}