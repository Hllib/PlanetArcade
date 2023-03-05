using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private TextMeshProUGUI _continuePanelText;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void ReadMoreButton()
    {
        string fileName = "linkToGit";
        string readFromFilePath = Application.streamingAssetsPath + "/" + fileName + ".txt";
        string link = "";

        List<string> lines = File.ReadAllLines(readFromFilePath).ToList();
        StringBuilder text = new StringBuilder();
        foreach (string line in lines)
        {
            text.AppendLine(line);
        }
        link = text.ToString();

        Application.OpenURL(link);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt(PlayerSettings.GameStarted, 0) == 1 && PlayerPrefs.GetInt(PlayerSettings.GameFinished, 0) == 0)
        {
            LoadScene("PlanetsMenu");
        }
        else
        {
            if (PlayerPrefs.GetInt(PlayerSettings.GameFinished, 0) == 1)
            {
                _continuePanelText.text = "You've completed your last playthrough. Start a new game";
            }
            else
            {
                _continuePanelText.text = "Apparently you have not started any game yet";
            }
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
        PlayerPrefs.SetInt(PlayerSettings.GameFinished, PlayerSettings.NotDone);
        PlayerPrefs.SetInt(PlayerSettings.GotAchievement, PlayerSettings.NotDone);
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
        PlayerPrefs.SetInt(PlayerSettings.GotAchievement, PlayerSettings.NotDone);
        _achievementPanel.SetActive(true);
    }

    private void Start()
    {   
        _soundPanel.SetActive(false);

        if (PlayerPrefs.GetInt(PlayerSettings.GotAchievement) == PlayerSettings.Done)
        {
            ShowAchievement();
        }
    }
}
