using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Can be used to access scriptable objects that the asset uses at runtime...
    /// </summary>
    public static class AssetAccessor
    {
        //
        //  Fields
        //
        
        
        private static AudioManagerAsset[] _cachedAssets;      // A cache of all the 

        
        //
        //  Properties
        //

        
        /// <summary>
        /// Gets all the assets of the scriptable object type AudioManagerAsset...
        /// </summary>
        private static IEnumerable<AudioManagerAsset> CachedAssets
        {
            get
            {
                if (_cachedAssets != null) return _cachedAssets;
                _cachedAssets = Resources.LoadAll("Audio Manager", typeof(AudioManagerAsset)).Cast<AudioManagerAsset>().ToArray();
                return _cachedAssets;
            }
        }
        
        
        //
        //  Methods
        //


        /// <summary>
        /// Gets the asset of the requested type...
        /// </summary>
        /// <typeparam name="T">The type to get...</typeparam>
        /// <remarks>Only works for scriptable objects than derive from AudioManagerAsset...</remarks>
        /// <returns>The result of the search...</returns>
        public static T GetAsset<T>() where T : AudioManagerAsset
        {
            return (T)CachedAssets.FirstOrDefault(t => t.GetType() == typeof(T));
        }
    }
}