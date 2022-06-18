using System.Collections.Generic;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    public static class AudioPool
    {
        private static AudioManagerSettings CachedSettings;
        private static Stack<GameObject> audioPrefabPool;
        private static List<AudioClipPlayer> activePrefabs;

        public static Stack<GameObject> PooledObjects => audioPrefabPool;
        public static List<AudioClipPlayer> ActiveObjects => activePrefabs;


        private static AudioManagerSettings Settings()
        {
            if (CachedSettings != null)
                return CachedSettings;
            
            CachedSettings = Resources.FindObjectsOfTypeAll<AudioManagerSettings>()[0];
            return CachedSettings;
        }
        
        public static GameObject GetPrefabFromPool(out AudioClipPlayer source)
        {
            if (audioPrefabPool == null)
            {
                audioPrefabPool = new Stack<GameObject>();
                activePrefabs = new List<AudioClipPlayer>();
            }
            
            GameObject _go;

            if (audioPrefabPool.Count > 0)
            {
                _go = audioPrefabPool.Pop();

                while (_go == null & audioPrefabPool.Count > 0)
                    _go = audioPrefabPool.Pop();

                if (_go == null)
                    _go = Object.Instantiate(Settings().Prefab);
                
                _go.SetActive(true);
            }
            else
                _go = Object.Instantiate(Settings().Prefab);

            if (_go.GetComponentInChildren<AudioClipPlayer>())
            {
                source = _go.GetComponentInChildren<AudioClipPlayer>();
                return _go;
            }

            source = null;
            return null;
        }
    }
}