using CarterGames.Assets.AudioManager;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnEnable()
    {

        AudioManager.Play(Clip.ui_menu_button_beep_01);

    }
}