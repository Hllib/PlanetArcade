using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _soundPanel;
    [SerializeField]
    private GameObject _soundPanelBg;

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
        PlayerPrefs.SetFloat(PlayerSettings.Earth, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Moon, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Mars, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Station, PlayerSettings.NewGame);
        PlayerPrefs.SetString(PlayerSettings.Inventory, string.Empty);
    }

    public void ShowSoundSettings()
    {
        _soundPanel.SetActive(!_soundPanel.activeSelf);
        _soundPanelBg.SetActive(!_soundPanelBg.activeSelf);
    }

    private void Start()
    {
        _soundPanel.SetActive(false);
    }
}
