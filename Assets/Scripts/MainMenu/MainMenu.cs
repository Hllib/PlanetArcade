using FMOD;
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
    private GameObject _menuCover;
    [SerializeField]
    private GameObject _continueMessagePanel;
    [SerializeField]
    private GameObject _newGameMessagePanel;
    [SerializeField]
    private GameObject _achievementPanel;
    [SerializeField]
    private GameObject _aboutPanel;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt(PlayerSettings.GameStarted, 0) == PlayerSettings.Done)
        {
            LoadScene("PlanetsMenu");
        }
        else
        {
            _menuCover.SetActive(true);
            _continueMessagePanel.SetActive(true);
        }
    }

    public void NewGame()
    {
        if (PlayerPrefs.GetInt(PlayerSettings.GameStarted, 0) == PlayerSettings.NotDone)
        {
            DeleteSaves();
            LoadScene("Station3D");
        }
        else
        {
            _menuCover.SetActive(true);
            _newGameMessagePanel.SetActive(true);
        }
    }

    public void ShowAboutPanel()
    {
        _menuCover.SetActive(true);
        _aboutPanel.SetActive(true);
    }

    public void CloseAboutPanel()
    {
        _menuCover.SetActive(false);
        _aboutPanel.SetActive(false);
    }

    public void CloseNewGameWarning()
    {
        _newGameMessagePanel.SetActive(false);
        _menuCover.SetActive(false);
    }

    public void CloseContinueMessage()
    {
        _continueMessagePanel.SetActive(false);
        _menuCover.SetActive(false);
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
        PlayerPrefs.SetInt(PlayerSettings.GameStarted, PlayerSettings.NotDone);
        PlayerPrefs.SetFloat(PlayerSettings.Earth, PlayerSettings.NotDone);
        PlayerPrefs.SetFloat(PlayerSettings.Moon, PlayerSettings.NotDone);
        PlayerPrefs.SetFloat(PlayerSettings.Mars, PlayerSettings.NotDone);
        PlayerPrefs.SetFloat(PlayerSettings.Station, PlayerSettings.NotDone);
        PlayerPrefs.SetString(PlayerSettings.Inventory, string.Empty);
    }

    public void ShowSoundSettings()
    {
        _soundPanel.SetActive(!_soundPanel.activeSelf);
        _menuCover.SetActive(!_menuCover.activeSelf);
    }

    private void ShowAchievement()
    {
        PlayerPrefs.SetInt(PlayerSettings.ShowAchievement, PlayerSettings.NotDone);
        _achievementPanel.SetActive(true);
    }

    private void Start()
    {
        _soundPanel.SetActive(false);

        if(PlayerPrefs.GetInt(PlayerSettings.ShowAchievement) == PlayerSettings.Done)
        {
            ShowAchievement();
        }
    }
}
