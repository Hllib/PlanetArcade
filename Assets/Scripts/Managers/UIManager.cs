using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private float _timeInGame;

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
        _gameOverScreen.SetActive(true);
    }

    public void ShowArtifact()
    {
        StartCoroutine(FinishCave());
    }

    IEnumerator FinishCave()
    {
        _foundArtifactPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _fadeToBlackCave.SetActive(true);
        yield return new WaitForSeconds(3.5f);

        GameManager.Instance.FinishCaveLevel();
    }

    public void UpdateHealthUI(int currentLives)
    {
        _animator.SetTrigger("Hit");
        for (int i = 0; i <= currentLives; i++)
        {
            if (i == currentLives)
            {
                _healthBars[i].enabled = false;
            }
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
        _soundPanel.SetActive(!_soundPanel.activeSelf);
    }
}
