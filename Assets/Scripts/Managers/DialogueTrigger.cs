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
    private TextAsset _inkJSON;

    public bool HasVisited;


    private void Awake()
    {
        _playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Start()
    {
        HasVisited = StationManager.Instance.HasPlayerVisited;
    }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.Instance.IsDialogueDisplayed && !HasVisited)
        {
            _visualCue.SetActive(true);
            if (_player.HasInteracted)
            {
                DialogueManager.Instance.StartDialogueMode(_inkJSON);
                _player.HasInteracted = false;
                HasVisited = true;

                if (GetComponentInParent<Merchant>() != null)
                {
                    Merchant merchant = GetComponentInParent<Merchant>();
                    merchant.TradeMode = true;
                    StationManager.Instance.HasPlayerTalkedToAll = true;

                    this.enabled = false;
                    _visualCue.SetActive(false);
                }
            }
        }
        else if (_playerInRange && !DialogueManager.Instance.IsDialogueDisplayed && HasVisited)
        {
            _visualCue.SetActive(false);
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
