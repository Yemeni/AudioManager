using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    public static class AssetAccessor
    {
        private static AudioManagerAsset[] assets;


        private static AudioManagerAsset[] Assets
        {
            get
            {
                if (assets != null) return assets;
                assets = Resources.LoadAll("Audio Manager", typeof(AudioManagerAsset)).Cast<AudioManagerAsset>().ToArray();
                return assets;
            }
        }


        public static T GetAsset<T>() where T : AudioManagerAsset
        {
            return (T)Assets.FirstOrDefault(t => t.GetType() == typeof(T));
        }
    }
}