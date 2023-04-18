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
    private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _player = collision.GetComponent<Player>();
            _player.FireBlocked = true;

            _playerInv = collision.GetComponent<Inventory>();
            _enterCavePanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _enterCavePanel.SetActive(false);
            _player.FireBlocked = false;
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
        _player.FireBlocked = false;
    }
}
