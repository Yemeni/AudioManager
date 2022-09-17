using UnityEngine;
using UnityEngine.Audio;

namespace CarterGames.Assets.AudioManager
{
    public class AudioManagerSettings : AudioManagerAsset
    {
        [SerializeField] private string version = "3.0.0";
        [SerializeField] private string releaseDate = "TBA";
        
        [SerializeField] private GameObject audioPrefab;
        [SerializeField] private AudioMixerGroup clipAudioMixer;
        
        [SerializeField] private GameObject musicPrefab;
        [SerializeField] private AudioMixerGroup musicAudioMixer;
        
        [SerializeField] private bool showDebugMessages;
        [SerializeField] private AudioMixerGroup[] additionalMixerGroups;
        [SerializeField] private float minVolumeVariance = -.1f;
        [SerializeField] private float maxVolumeVariance = .1f;
        [SerializeField] private float minPitchVariance = -.1f;
        [SerializeField] private float maxPitchVariance = .1f;
        [SerializeField] private int numberPerPageInEditor = 10;

        [SerializeField, HideInInspector] private int editorTabPosition;
        [SerializeField, HideInInspector] private bool showVariance;
        [SerializeField, HideInInspector] private bool showEditorOptions;

        public GameObject Prefab => audioPrefab;
        public GameObject MusicPrefab => musicPrefab;
        public AudioMixerGroup ClipAudioMixer => clipAudioMixer;
        public AudioMixerGroup MusicAudioMixer => musicAudioMixer;
        public bool ShowDebugMessages => showDebugMessages;
        public AudioMixerGroup[] AdditionalMixerGroups => additionalMixerGroups;

        public bool CanPlayAudio = true;
        public bool CanPlayMusic = true;

        public float variantVolume => Random.Range(minVolumeVariance, maxVolumeVariance);
        public float variantPitch => Random.Range(minPitchVariance, maxPitchVariance);
        
        
        /// <summary>
        /// Sets up the settings object for use by setting the default prefabs....
        /// </summary>
        public void InitialiseSettings(GameObject audioPrefab, GameObject musicPrefab)
        {
            this.audioPrefab = audioPrefab;
            this.musicPrefab = musicPrefab;
        }
    }
}