using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class AudioClipInfo
    {
        [SerializeField] private string clipId;
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource clipSource;

        public string ClipID => clipId;
        public string ClipName => clip.name;
        public AudioClip Clip => clip;
        public AudioSource AudioSource => clipSource;
        public GameObject GameObject => clipSource.gameObject;
        
        
        public AudioClipInfo() { }

        
        public AudioClipInfo(AudioSource source)
        {
            clipSource = source;
            clip = source.clip;
        }
        
        public AudioClipInfo(AudioSource source, AudioClip clip)
        {
            clipSource = source;
            this.clip = clip;
        }

        public void SetClipId(string id)
        {
            clipId = id;
        }

        public void PauseClip()
        {
            clipSource.Pause();
        }
        
        public void StopClip()
        {
            clipSource.Stop();
        }
    }
}