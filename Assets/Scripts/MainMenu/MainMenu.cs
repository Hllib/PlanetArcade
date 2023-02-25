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
        StartCoroutine(Load(sceneName));
    }

    IEnumerator Load(string sceneName)
    {
        yield return new WaitForSeconds(0.4f);
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

    public void ShowSoundSettings()
    {
        _soundPanel.SetActive(!_soundPanel.activeSelf);
        _soundPanelBg.SetActive(!_soundPanelBg.activeSelf);
    }
}
