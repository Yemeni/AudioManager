using System.Collections.Generic;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Handles the object pooling for audio players....
    /// </summary>
    public static class AudioPool
    {
        //
        //  Fields
        //
        
        
        private static AudioManagerSettings _cachedSettings;          // Holds a cache of the settings asset so it doesn't get it more than once...
        private static Stack<AudioClipPlayer> _inactivePrefabPool;    // Holds all objects not in use...
        private static List<AudioClipPlayer> _activePrefabPool;       // Holds all active clips...
        private static GameObject _doNotDestroyParent;                // Holds the parent object for the pool which is in the do not destroy scene...
        

        //
        //  Properties
        //
        
        
        /// <summary>
        /// Gets all the objects currently in the pool that are not in use...
        /// </summary>
        public static Stack<AudioClipPlayer> PooledObjects => _inactivePrefabPool;
        
        
        /// <summary>
        /// Gets all the objects currently playing audio...
        /// </summary>
        public static List<AudioClipPlayer> ActiveObjects => _activePrefabPool;

        
        /// <summary>
        /// Gets the settings asset if the cache doesn't already have it...
        /// </summary>
        /// <returns></returns>
        private static AudioManagerSettings Settings()
        {
            if (_cachedSettings != null)
                return _cachedSettings;

            _cachedSettings = AssetAccessor.GetAsset<AudioManagerSettings>();
            return _cachedSettings;
        }
        
        
        //
        //  Methods
        //


        /// <summary>
        /// Runs before any game logic runs and initializes the pools for use with some objects in them by default...
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialise()
        {
            // Creates the parent object for the pool...
            _doNotDestroyParent = new GameObject("Audio Pool - (Audio Manager | CG)");
            Object.DontDestroyOnLoad(_doNotDestroyParent);
            
            // Initialises the pool collections...
            _inactivePrefabPool = new Stack<AudioClipPlayer>();
            _activePrefabPool = new List<AudioClipPlayer>();

            // Initialises the initial pool so there are a few ready to go for the user...
            for (var i = 0; i < 5; i++)
            {
                if (!Object.Instantiate(Settings().Prefab).TryGetComponent(out AudioClipPlayer playerAttached)) continue;
                playerAttached.name = "Audio Clip Player (Instance)";
                playerAttached.transform.SetParent(_doNotDestroyParent.transform);
                playerAttached.gameObject.SetActive(false);
                _inactivePrefabPool.Push(playerAttached);
            }
        }


        /// <summary>
        /// Gets a prefab from the pool of available objects...
        /// </summary>
        /// <returns>The next audio clip player for use...</returns>
        public static AudioClipPlayer GetPrefabFromPool()
        {
            // If there are no free in the pool to use...
            if (_inactivePrefabPool.Count > 0)
            {
                return _inactivePrefabPool.Pop();
            }
            
            // Tries to make a new player if it is possible...
            Object.Instantiate(Settings().Prefab).TryGetComponent(out AudioClipPlayer playerAttached);
            playerAttached.name = "Audio Clip Player (Instance)";
            playerAttached.transform.SetParent(_doNotDestroyParent.transform);

            if (playerAttached)
            {
                _activePrefabPool.Add(playerAttached);
                return playerAttached;
            }

            // Error message is handled by callers of this method so no need to have one here...
            return null;
        }


        /// <summary>
        /// Returns a player to the pool for re-use...
        /// </summary>
        /// <param name="clipPlayer">The player to return...</param>
        public static void Return(AudioClipPlayer clipPlayer)
        {
            _activePrefabPool.Remove(clipPlayer);
            _inactivePrefabPool.Push(clipPlayer);
            clipPlayer.gameObject.SetActive(false);
        }
    }
}