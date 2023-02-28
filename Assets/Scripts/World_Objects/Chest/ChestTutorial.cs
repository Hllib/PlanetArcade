using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _lootPrefab;
    private Animator _animator;
    private bool _isLooted;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isLooted)
        {
            Inventory _playerInv = collision.GetComponent<Inventory>();

            if (_playerInv != null)
            {
                if (_playerInv.playerItems.Any(item => item.id == InventoryTypes.Key))
                {
                    _isLooted = true;
                    _animator.enabled = false;

                    _playerInv.RemoveItem(InventoryTypes.Key);
                    Instantiate(_lootPrefab, transform.position, Quaternion.identity);

                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.chestOpen, this.transform.position);
                    Guide.Instance.EnableAmmo();
                }
                else
                {
                    UIManager.Instance.DisplayMessage("You need to have a key!");
                }
            }
        }

    }
}
