using CarterGames.Assets.AudioManager;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnEnable()
    {
    
        
        
        AudioManager.Play(Clip.Click_01);
        AudioManager.Play(Clip.Click_01, .95f, 1f);
        AudioManager.Play(Clip.Click_01, true, Mixer.Master);
        AudioManager.Play(Group.Clicks);
        AudioManager.Play(Group.Clicks, .95f, 1f);
        AudioManager.Play(Group.Clicks, true, Mixer.Master);
        
        
    }
}