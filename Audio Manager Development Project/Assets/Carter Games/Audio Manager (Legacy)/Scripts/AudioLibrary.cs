using System;
using UnityEngine;

/*
 * 
 *  Audio Manager
 *							  
 *	Audio Library
 *      Used to store the clips in the scriptable object Audio Manager Files.
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

namespace CarterGames.Legacy.AudioManager
{
    /// <summary>
    /// Used the store the key/pair values for each clip found in the audio manager scan.
    /// </summary>
    [Serializable]
    public class AudioLibrary
    {
        public string path;
        public string key;
        public AudioClip value;

        public AudioLibrary(string path, string key, AudioClip value)
        {
            this.path = path;
            this.key = key;
            this.value = value;
        }
    }
}