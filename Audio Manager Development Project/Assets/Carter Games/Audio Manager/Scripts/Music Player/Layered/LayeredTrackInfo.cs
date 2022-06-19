using System;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public struct LayeredTrackInfo
    {
        public string id;
        public AudioData data;
        public AudioSettings settings;
        public float startTime;
        public float endTime;
    }
}