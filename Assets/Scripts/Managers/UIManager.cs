using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image[] _healthBars;
    [SerializeField]
    private GameObject _messagePrefab;
    [SerializeField]
    private Transform _messageHolder;
    [SerializeField]
    private GameObject _gameOverScreen;
    [SerializeField]
    private GameObject _foundArtifactPanel;
    [SerializeField]
    private GameObject _fadeToBlackCave;
    [SerializeField]
    private GameObject _soundPanel;
    [SerializeField]
    private TextMeshProUGUI _ammoText;
    [SerializeField]
    private Player _player;

    private Animator _animator;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _soundPanel.SetActive(false);
    }

    public void ShowGameOverScren()
    {
        _player.FireBlocked = true;
        _gameOverScreen.SetActive(true);
    }

    public void ShowArtifact(int type)
    {
        switch(type)
        {
            case InventoryTypes.ArtifactMoon: StartCoroutine(FinishCave());  break;
            case InventoryTypes.ArtifactMars: StartCoroutine(FinishMars()); break;
        }
    }

    IEnumerator FinishMars()
    {
        _foundArtifactPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        _foundArtifactPanel.SetActive(false);
    }

    IEnumerator FinishCave()
    {
        _foundArtifactPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _fadeToBlackCave.SetActive(true);
        yield return new WaitForSeconds(3.5f);

        GameManager.Instance.FinishMoon();
    }

    public void UpdateHealthUI(int currentLives)
    {
        int diff = PlayerSettings.Health - (PlayerSettings.Health - currentLives);

        _animator.SetTrigger("Hit");
        foreach(var bar in _healthBars)
        {
            bar.enabled = false;
        }

        for(int i = 0; i < diff; i++)
        {
            _healthBars[i].enabled = true;
        }
    }

    public void DisplayMessage(string message)
    {
        char[] characters = message.ToCharArray();

        GameObject messageUi = Instantiate(_messagePrefab);
        messageUi.transform.SetParent(_messageHolder, false);

        StartCoroutine(PrintMessage(characters, messageUi));
    }

    IEnumerator PrintMessage(char[] characters, GameObject messageUi)
    {
        Text messageUiText = messageUi.GetComponentInChildren<Text>();
        for (int i = 0; i < characters.Length; i++)
        {
            messageUiText.text += characters[i];
            if (characters[i] != ' ')
            {
                yield return new WaitForSeconds(0.08f);
            }
        }

        yield return new WaitForSeconds(3f);
        Destroy(messageUi);
        messageUiText.text = string.Empty;
    }

    public void ShowSoundPanel()
    {
        if(_player != null)
        {
            if (_soundPanel.activeSelf == true) //closing the panel
            {
                _player.FireBlocked = false;
            }
            else //opening the panel
            {
                _player.FireBlocked = true;
            }
        }
        _soundPanel.SetActive(!_soundPanel.activeSelf);
    }

    public void UpdateAmmoCount(int amount)
    {
        _ammoText.text = amount.ToString();
    }
}
