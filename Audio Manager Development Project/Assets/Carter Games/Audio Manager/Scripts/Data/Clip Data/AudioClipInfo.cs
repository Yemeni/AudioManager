using System;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// A data class managing an instance of a clip that is actively being played...
    /// </summary>
    [Serializable]
    public class AudioClipInfo
    {
        //
        //  Fields
        //
        
        
        [SerializeField] private string clipId;                         // The id for the this info class...
        [SerializeField] private AudioClip audioClip;                   // The clip the class is playing...
        [SerializeField] private AudioSource audioSource;               // The source the clip is playing on...
        [SerializeField] private float clipLenghtOffset;                // The offset for methods like delay or from time...

        
        //
        //  Properties
        //
        
        
        /// <summary>
        /// The clip Id of this info class...
        /// </summary>
        public string ClipID => clipId;
        
        
        /// <summary>
        /// The name of the clip on this info class...
        /// </summary>
        public string ClipName => audioClip.name;
        
        
        /// <summary>
        /// The audio clip stored on this info class...
        /// </summary>
        public AudioClip AudioClip => audioClip;
        
        
        /// <summary>
        /// The audio source used by this info class...
        /// </summary>
        public AudioSource AudioSource => audioSource;
        
        
        /// <summary>
        /// The gameObject this info class is on...
        /// </summary>
        public GameObject GameObject => audioSource.gameObject;


        /// <summary>
        /// The amount of additional time needed for the clip to fully play...
        /// </summary>
        public float ClipLenghtOffset => clipLenghtOffset;


        //
        //  Events
        //

        
        /// <summary>
        /// Raises when the clip is resumed by the user...
        /// </summary>
        public Evt OnClipResumed = new Evt();
        
        
        /// <summary>
        /// Raises when the clip is paused by the user...
        /// </summary>
        public Evt OnClipPaused = new Evt();
        
        
        /// <summary>
        /// Raised when the clip is stopped by the user...
        /// </summary>
        public Evt OnClipStopped = new Evt();


        //
        //  Constructors
        //
        
        
        /// <summary>
        /// Creates a new instance of the audio clip info class with the audio source set & an offset for the clip length...
        /// </summary>
        /// <param name="source">The AudioSource this instance is for...</param>
        /// <param name="clipLenghtOffset">The offset to the lenght of the clip... (Used to delay the recycling of clips for a little bit)</param>
        public AudioClipInfo(AudioSource source, float clipLenghtOffset = 0)
        {
            audioSource = source;
            audioClip = source.clip;
            this.clipLenghtOffset = clipLenghtOffset;
        }
        
        
        //
        //  Methods
        //


        /// <summary>
        /// Sets the clip id of this clip to the entered value...
        /// </summary>
        /// <param name="id">The id to set...</param>
        public void SetClipId(string id) => clipId = id;
        

        /// <summary>
        /// Resumes the clip if it is paused...
        /// </summary>
        /// <remarks>This will only function if the clip is paused... If it is not nothing will happen...</remarks>
        public void ResumeClip()
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
            OnClipResumed.Raise();
        }

        
        /// <summary>
        /// Paused the clip if it is currently playing...
        /// </summary>
        /// <remarks>This will only function if the clip is playing... If it is not nothing will happen...</remarks>
        public void PauseClip()
        {
            audioSource.Pause();
            OnClipPaused.Raise();
        }
        
        
        /// <summary>
        /// Stops the clip if it is currently playing...
        /// </summary>
        /// <remarks>This will only function if the clip is playing... If it is not nothing will happen...</remarks>
        public void StopClip()
        {
            audioSource.Stop();
            OnClipStopped.Raise();
        }
    }
}