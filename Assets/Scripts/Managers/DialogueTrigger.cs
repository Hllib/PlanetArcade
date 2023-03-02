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

    public bool HasTalked;


    private void Awake()
    {
        _playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.Instance.IsDialogueDisplayed && !HasTalked)
        {
            _visualCue.SetActive(true);
            if (_player.HasInteracted)
            {
                DialogueManager.Instance.StartDialogueMode(_inkJSON);
                _player.HasInteracted = false;
                HasTalked = true;
                if (GetComponentInParent<Merchant>() != null)
                {
                    Merchant merchant = GetComponentInParent<Merchant>();
                    merchant.TradeMode = true;

                    this.enabled = false;
                    _visualCue.SetActive(false);
                }
            }
        }
        else
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
