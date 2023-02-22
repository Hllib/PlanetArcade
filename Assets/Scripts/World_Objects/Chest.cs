using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : MonoBehaviour
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
                _isLooted = true;
                _animator.enabled = false;
                Instantiate(_lootPrefab, transform.position, Quaternion.identity);
            }
        }

    }
}
