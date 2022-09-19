using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// MonoBehaviour Class | The audio player, designed to play audio from an AMF from a UI object.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        // The file to read and use in the player...
        [SerializeField] private AudioManagerFile audioManagerFile = default;
        [SerializeField] private AudioMixerGroup mixer = default;
        
        // Used to define what the script will play xD
        [SerializeField] private List<AudioPlayerData> clipsToPlay = default;
        [SerializeField] private Vector2 scrollPos;

        // An instance of the library to use in the 
        private Dictionary<string, AudioClip> lib;
        private AudioManager am;
        
        

        private void Start()
        {
#if Use_CGAudioManager_Static || USE_CG_AM_STATIC
            am = AudioManager.instance;
#else
            am = FindObjectOfType<AudioManager>();
#endif
            
            lib = new Dictionary<string, AudioClip>();

            foreach (var _t in audioManagerFile.library)
                lib.Add(_t.key, _t.value);
        }


        /// <summary>
        /// Plays the clip(s) selected in the inspector as they are with the volume/pitch/mixer from the inspector.
        /// </summary>
        public void Play()
        {
            if (audioManagerFile.library == null) return;
            {
                for (int i = 0; i < clipsToPlay.Count; i++)
                {
                    if (lib.ContainsKey(clipsToPlay[i].clipName))
                    {
                        AudioSource _clip;

                        if (!am)
                        {
#if Use_CGAudioManager_Static || USE_CG_AM_STATIC
                            am = AudioManager.instance;
#else
                            am = FindObjectOfType<AudioManager>();
#endif
                        }

                        _clip = AudioPool.Assign();
                        _clip.gameObject.SetActive(true);
                        
                        if (!_clip) return;

                        var _source = _clip.GetComponent<AudioSource>();
                        var _audioRemoval = _source.GetComponent<AudioClipPlayer>();

                        _source.clip = lib[clipsToPlay[i].clipName];
                        // _source.volume = clipsVolume[i];
                        // _source.pitch = clipsPitch[i];

                        if (clipsToPlay[i].fromTime > 0)
                            _source.time = clipsToPlay[i].fromTime;

                        _source.outputAudioMixerGroup = mixer;
                        
                        if (clipsToPlay[i].clipDelay > 0)
                            _source.PlayDelayed(clipsToPlay[i].clipDelay);
                        else
                            _source.Play();
                        
                        _audioRemoval.Cleanup(_source.clip.length);
                    }
                    else
                    {
                        AmLog.Warning("Could not find clip. Please ensure the clip is scanned and the string you entered is correct (Note the input is CaSe SeNsItIvE).");
                    }
                }
            }
        }
    }
}