using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    public static class MusicManager
    {
        private static AudioLibrary CachedLibrary;
        private static AudioManagerSettings CachedSettings;
        private static Dictionary<string, AudioData> CachedDictionary;
        private static int ActiveTrackCount { get; set; }
        private static readonly string ActiveTrackPrefix = "Track_";

        private static AudioLibrary Library
        {
            get 
            {
                if (CachedLibrary == null)
                    CachedLibrary = Resources.Load("Audio Manager/Audio Library") as AudioLibrary;

                return CachedLibrary;
            }
        }
        
        private static AudioManagerSettings Settings
        {
            get 
            {
                if (CachedSettings == null)
                    CachedSettings = Resources.Load("Audio Manager/Audio Manager Settings") as AudioManagerSettings;

                return CachedSettings;
            }
        }
        
        private static Dictionary<string, AudioData> Dictionary
        {
            get
            {
                if (CachedDictionary != null) return CachedDictionary;
                
                var lib = Library.GetData;
                CachedDictionary = new Dictionary<string, AudioData>();

                foreach (var data in lib)
                    CachedDictionary.Add(data.key, data);

                return CachedDictionary;
            }
        }
        
        
        
        public static MusicTrackInfo Play(string track, MusicTransitionType transitionType)
        {
            if (!Settings.CanPlayMusic) return null;
            
            return null;
        }
        

        public static void Stop()
        {
            
        }
    }
}