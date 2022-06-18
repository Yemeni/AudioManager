/*
 * 
 *  Audio Manager
 *							  
 *	Audio Manager Pool
 *      Handles the object pooling for the audio manager.
 *			
 *  Written by:
 *      Jonathan Carter
 *
 *  Published By:
 *      Carter Games
 *      E: hello@carter.games
 *      W: https://www.carter.games
 *		
 */

using System.Collections.Generic;
using UnityEngine;

namespace CarterGames.Legacy.AudioManager
{
    public static class AudioManagerPool
    {
        private static Stack<GameObject> audioPrefabPool;
        private static List<AudioSource> activePrefabs;

        public static Stack<GameObject> Pool => audioPrefabPool;
        public static List<AudioSource> Active => activePrefabs;


        /// <summary>
        /// Runs the common code for getting a prefab to play the audio on...
        /// </summary>
        public static GameObject GetPrefabFromPool(GameObject prefab)
        {
            if (audioPrefabPool == null)
            {
                audioPrefabPool = new Stack<GameObject>();
                activePrefabs = new List<AudioSource>();
            }
            
            GameObject _go;

            if (audioPrefabPool.Count > 0)
            {
                _go = audioPrefabPool.Pop();

                while (_go == null & audioPrefabPool.Count > 0)
                {
                    _go = audioPrefabPool.Pop();
                }
                
                if (_go == null)
                    _go = Object.Instantiate(prefab);
                
                _go.SetActive(true);
            }
            else
                _go = Object.Instantiate(prefab);

            if (!_go.GetComponent<AudioSource>())
            {
                Debug.LogWarning(
                    "* Audio Manager * | Warning Code 4 | No AudioSource Component found on the Sound Prefab. Please ensure a AudioSource Component is attached to your prefab.");
                return null;
            }

            return _go;
        }
    }
}