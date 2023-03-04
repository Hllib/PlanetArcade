using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Player3D _player;
    private bool _playerInRange;
    [SerializeField]
    private GameObject _visualCue;

    [SerializeField]
    private TextAsset _introDialogJSON;
    [SerializeField]
    private TextAsset _commonDialogJSON;
    [SerializeField]
    private TextAsset _finalDialogJSON;

    public bool HasTalkedTo;
    private bool _isGameFinished;

    private void Awake()
    {
        _playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Start()
    {
        HasTalkedTo = StationManager.Instance.HasPlayerVisited;
        _isGameFinished = PlayerPrefs.GetInt(PlayerSettings.GameFinished, 0) == 1 ? true : false;
        _isGameFinished = true;
    }

    private void Update()
    {
        if (!DialogueManager.Instance.FinalDialogeCompleted)
        {
            if (_isGameFinished && _playerInRange && !DialogueManager.Instance.IsDialogueDisplayed)
            {
                CheckForFinalDialoge();
            }

            if (_playerInRange && !DialogueManager.Instance.IsDialogueDisplayed && !HasTalkedTo)
            {
                CheckForFirstDialoge();
            }
            else if (_playerInRange && !DialogueManager.Instance.IsDialogueDisplayed && HasTalkedTo)
            {
                CheckForCommonDialoge();
            }
        }
    }

    private void CheckForFirstDialoge()
    {
        _visualCue.SetActive(true);
        if (_player.HasInteracted)
        {
            DialogueManager.Instance.StartDialogueMode(_introDialogJSON);
            _player.HasInteracted = false;
            HasTalkedTo = true;

            if (GetComponentInParent<Merchant>() != null)
            {
                Merchant merchant = GetComponentInParent<Merchant>();
                merchant.TradeMode = true;
                StationManager.Instance.HasPlayerTalkedToAll = true;

                _visualCue.SetActive(false);
                this.enabled = false;
            }
        }

    }

    private void CheckForCommonDialoge()
    {
        _visualCue.SetActive(true);
        if (_player.HasInteracted)
        {
            DialogueManager.Instance.StartDialogueMode(_commonDialogJSON);
            _player.HasInteracted = false;
        }
    }

    private void CheckForFinalDialoge()
    {
        if (!DialogueManager.Instance.FinalDialogeCompleted)
        {
            DialogueManager.Instance.StartDialogueMode(_finalDialogJSON);
            _player.HasInteracted = false;
            DialogueManager.Instance.FinalDialogeCompleted = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.eButton, this.transform.position);
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}
