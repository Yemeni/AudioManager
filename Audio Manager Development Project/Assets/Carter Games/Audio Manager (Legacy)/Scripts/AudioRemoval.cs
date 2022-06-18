using System.Collections;
using UnityEngine;

/*
 * 
 *  Audio Manager
 *							  
 *	Audio Removal Script
 *      A clean up script that disables audio clip instances when the clip has finished playing for reuse.
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
    public class AudioRemoval : MonoBehaviour
    {
        private static AudioManagerFile am;


        private void OnDisable()
        {
            StopAllCoroutines();
        }
        

        /// <summary>
        /// Disables the clip entered once it has completed a play-through.
        /// </summary>
        /// <param name="clip">GameObject | The audio prefab to disable.</param>
        /// <param name="time">Float | The amount of the to wait before disabling the object.</param>
        public void Cleanup(AudioManagerFile file, GameObject clip, float time)
        {
            am = file;
            StartCoroutine(CleanupCo(clip, time));
        }


        /// <summary>
        /// Coroutine | Runs the cleanup when the correct amount of time has passed.
        /// </summary>
        /// <param name="clip">GameObject | The audio prefab to disable.</param>
        /// <param name="time">Float | The amount of the to wait before disabling the object.</param>
        private IEnumerator CleanupCo(GameObject clip, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            AudioManagerPool.Pool.Push(clip);
            AudioManagerPool.Active.Remove(clip.GetComponent<AudioSource>());
            clip.SetActive(false);
        }
    }
}