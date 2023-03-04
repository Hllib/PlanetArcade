using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI _dialogueText;
    [SerializeField]
    private Story _currentStory;
    [SerializeField]
    private Player3D _player;
    [SerializeField]
    private Animator _playerAnimator;
    [SerializeField]
    private GameObject[] _choices;
    private TextMeshProUGUI[] _choicesText;
    [SerializeField]
    private GameObject _scrollbar;

    public bool IsDialogueDisplayed { get; private set; }

    public bool HasSubmitted { get; private set; }

    public bool FinalDialogeCompleted { get; set; }

    private static DialogueManager _instance;

    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("DialogueManager is NULL!::DialogueManager.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        IsDialogueDisplayed = false;
        _dialoguePanel.SetActive(false);

        _choicesText = new TextMeshProUGUI[_choices.Length];

        for (int i = 0; i < _choices.Length; i++)
        {
            _choicesText[i] = _choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        _playerAnimator = _player.GetComponent<Animator>();
    }

    public void StartDialogueMode(TextAsset inkJSON)
    {
        _playerAnimator.SetBool("Idle", true);

        _currentStory = new Story(inkJSON.text);
        IsDialogueDisplayed = true;
        _dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void StopDialogueMode()
    {
        _playerAnimator.SetBool("Idle", false);

        IsDialogueDisplayed = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = string.Empty;

        if(FinalDialogeCompleted)
        {
            StationManager.Instance.PlayerFinishedFinalDialoge = true;
        }
    }

    private void Update()
    {
        if (!IsDialogueDisplayed)
        {
            return;
        }

        if (HasSubmitted)
        {
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _scrollbar.GetComponent<Scrollbar>().value = 1;
            HasSubmitted = false;
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            StopDialogueMode();
            HasSubmitted = false;
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count == 0)
        {
            StopDialogueMode();
            HasSubmitted = false;
        }

        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("More choices given than UI can support::DialogueManager.cs");
        }

        //enable and init choices 
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        //loop through remaining choices in UI and hide them
        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        HasSubmitted = true;
        _currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
