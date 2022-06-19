// ----------------------------------------------------------------------------
// MusicTrackInfo.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 10/06/2022
// ----------------------------------------------------------------------------

using System;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class MusicTrackInfo
    {
        [SerializeField] private string trackId;
        [SerializeField] private AudioClip track;
        [SerializeField] private AudioSource activeSource;
        
        private AudioSource inactiveSource;
        private AudioSource[] sources;
        private float loopPoint;
        private float startPoint;
        private float endPoint;

        public string TrackId => trackId;
        public string TrackName => track.name;
        public AudioClip Track => track;
        public AudioSource ActiveSource => activeSource;
        public AudioSource InactiveSource => inactiveSource;
        

        public MusicTrackInfo(AudioSource[] sources, AudioClip track)
        {
            this.sources = sources;
            activeSource = sources[0];
            inactiveSource = sources[1];
            this.track = track;
        }

        public void SetTrackId(string id)
        {
            trackId = id;
        }

        public void PauseTrack()
        {
            if (activeSource.isPlaying)
                activeSource.Pause();
            
            if (!inactiveSource.isPlaying) return;
            inactiveSource.Pause();
        }
        
        public void StopTrack()
        {
            if (activeSource.isPlaying)
                activeSource.Stop();
            
            if (!inactiveSource.isPlaying) return;
            inactiveSource.Stop();
        }

        public void SwitchActiveSource()
        {
            activeSource = inactiveSource;
            inactiveSource = sources.First(t => activeSource.Equals(t));
        }
    }
}