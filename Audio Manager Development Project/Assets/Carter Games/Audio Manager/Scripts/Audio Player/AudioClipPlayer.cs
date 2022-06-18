using System.Collections;
using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipPlayer : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField] private AudioClipInfo clipInfo;

        public AudioSource Source => source;
        public AudioClipInfo ClipInfo => clipInfo;
        
        private void OnEnable()
        {
            source = GetComponentInChildren<AudioSource>();
        }
        
        public void SetData(AudioClipInfo clip)
        {
            clipInfo = clip;
            if (clip.AudioSource.loop) return;
            StartCoroutine(Co_Removal());
        }

        private IEnumerator Co_Removal()
        {
            yield return new WaitForSecondsRealtime(source.clip.length);
            AudioPool.PooledObjects.Push(gameObject);
            AudioPool.ActiveObjects.Remove(this);
            AudioEvents.OnClipStopped.Raise(clipInfo);
        }
    }
}