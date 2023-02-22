using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{
    [SerializeField]
    private GameObject _enterCavePanel;
    private Inventory _playerInv;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerInv = collision.GetComponent<Inventory>();
            _enterCavePanel.SetActive(true);
        }
    }

    public void EnterCave()
    {
        if(_playerInv.playerItems.Any(item => item.id == InventoryTypes.Key))
        {
            _playerInv.RemoveItem(InventoryTypes.Key);
        }
        GameManager.Instance.SavePlayerPrefs();
        SceneManager.LoadScene("Cave");
    }

    public void Leave()
    {
        _enterCavePanel.SetActive(false);
    }
}
