using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image _soundButtonImg;
    [SerializeField]
    private Sprite[] _soundStateSprites;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
         Application.Quit();
        #endif
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SwitchSound()
    {
        switch(AudioListener.volume)
        {
            case 0: AudioListener.volume = 1; _soundButtonImg.sprite = _soundStateSprites[1]; break;
            case 1: AudioListener.volume = 0; _soundButtonImg.sprite = _soundStateSprites[0]; break;
        }
    }
}
