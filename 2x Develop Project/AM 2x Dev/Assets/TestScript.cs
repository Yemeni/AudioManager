using CarterGames.Assets.AudioManager;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestScript : MonoBehaviour
    {
        
        
        public void PlayMusic()
        {
            MusicPlayer.instance.ToggleMusic(true);
        }


        public void StopMusic()
        {
            MusicPlayer.instance.ToggleMusic(false);
        }
    }
}