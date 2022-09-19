using System;

namespace CarterGames.Assets.AudioManager
{
    [Serializable]
    public class AudioPlayerData
    {
        public bool show;
        public string clipName;
        
        // Volume
        public MinMaxFloat volume;
        public MinMaxFloat pitch;

        // Optional Settings
        public bool showOptional;
        public float fromTime;
        public float clipDelay;
    }
}