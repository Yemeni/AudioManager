using System.Collections;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Plays audio clips for their lifetime and then returns itself to the manager for re-use...
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipPlayer : MonoBehaviour
    {
        //
        //  Fields
        //
        
        
        private AudioClipInfo clipInfo;                         // The clip info to play...
        private Coroutine playerRoutine;                        // The active coroutine for this player...
        private float clipTimeRemaining;                        // The time left on the clip to play...
        private float clipLifetimeDuration;                     // The lenght of time this clip is to play for...
        
        
        //
        //  Properties
        //
        
        
        /// <summary>
        /// Gets the audio source on this player...
        /// </summary>
        public AudioSource AudioSource { get; private set; }
        
        
        /// <summary>
        /// Get the clip info for this player...
        /// </summary>
        public AudioClipInfo ClipInfo
        {
            get => clipInfo;
            set => clipInfo = value;
        }


        //
        //  Unity Methods
        //


        private void OnEnable() => Initialise();
        
        private void OnDisable() => Dispose();
        

        //
        //  Methods
        //


        /// <summary>
        /// Initialises the player for use...
        /// </summary>
        private void Initialise()
        {
            if (AudioSource == null)
                AudioSource = GetComponentInChildren<AudioSource>();
            
            if (clipInfo == null) return;
            clipInfo.OnClipPaused.Add(OnClipPausedManually);
            clipInfo.OnClipResumed.Add(OnClipResumedManually);
            clipInfo.OnClipStopped.Add(OnClipStoppedManually);
        }


        /// <summary>
        /// Disposes of the player safely...
        /// </summary>
        private void Dispose()
        {
            if (clipInfo == null) return;
            clipInfo.OnClipPaused.Remove(OnClipPausedManually);
            clipInfo.OnClipResumed.Remove(OnClipResumedManually);
            clipInfo.OnClipStopped.Remove(OnClipStoppedManually);
        }
        
        
        /// <summary>
        /// Sets the data for the player to use to play the requested clip...
        /// </summary>
        /// <param name="clip">THe info of the clip to play...</param>
        public void SetData(AudioClipInfo clip)
        {
            clipInfo = clip;
            if (clip.AudioSource.loop) return;
            clipTimeRemaining = clip.AudioClip.length + clipInfo.ClipLenghtOffset;
            clipLifetimeDuration = clipTimeRemaining;
            gameObject.SetActive(true);
            playerRoutine = StartCoroutine(Co_Removal());
        }
        
        
        /// <summary>
        /// Runs when the info class has been called to resume the clip...
        /// </summary>
        private void OnClipResumedManually()
        {
            if (playerRoutine == null) return;
            playerRoutine = StartCoroutine(Co_Removal());
        }
        

        /// <summary>
        /// Runs when the info class has been called to pause the clip...
        /// </summary>
        private void OnClipPausedManually()
        {
            if (playerRoutine == null) return;
            StopCoroutine(playerRoutine);
            clipTimeRemaining = clipLifetimeDuration - AudioSource.time;
        }


        /// <summary>
        /// Runs when the info class has been called to stop the clip...
        /// </summary>
        private void OnClipStoppedManually()
        {
            if (playerRoutine == null) return;
            StopCoroutine(playerRoutine);
            clipTimeRemaining = clipLifetimeDuration - AudioSource.time;
        }
        
        
        //
        //  Coroutines
        //

        
        /// <summary>
        /// Runs cleanup for the player so when the clip has completed its playing it will return to the manager for re-use...
        /// </summary>
        private IEnumerator Co_Removal()
        {
            yield return new WaitForSecondsRealtime(clipTimeRemaining);
            AudioPool.Return(this);
            AudioEvents.OnClipFinishedPlaying.Raise(clipInfo);
        }
    }
}