using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    public static class AssetAccessor
    {
        private static AudioManagerAsset[] assets;


        private static IEnumerable<AudioManagerAsset> Assets
        {
            get
            {
                if (assets != null) return assets;
                assets = Resources.LoadAll<AudioManagerAsset>("Assets/Resources");
                return assets;
            }
        }


        public static T GetAsset<T>() where T : AudioManagerAsset
        {
            return (T)Assets.FirstOrDefault(t => t.GetType() == typeof(T));
        }
    }
}