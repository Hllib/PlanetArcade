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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt(PlayerSettings.GameStarted, 0) == PlayerSettings.LevelFinished)
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
        if (PlayerPrefs.GetInt(PlayerSettings.GameStarted, 0) == PlayerSettings.NewGame)
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
        PlayerPrefs.SetFloat(PlayerSettings.Earth, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Moon, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Mars, PlayerSettings.NewGame);
        PlayerPrefs.SetFloat(PlayerSettings.Station, PlayerSettings.NewGame);
        PlayerPrefs.SetString(PlayerSettings.Inventory, string.Empty);
    }

    public void ShowSoundSettings()
    {
        _soundPanel.SetActive(!_soundPanel.activeSelf);
        _menuCover.SetActive(!_menuCover.activeSelf);
    }

    private void Start()
    {
        _soundPanel.SetActive(false);
    }
}
