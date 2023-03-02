using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortToScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _fade;

    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _player = collision.GetComponent<Player>();

            switch(_sceneName)
            {
                case "BossFight": StartCoroutine(MoveToBoss()); break;   
                case "Final": StartCoroutine(MoveToBoss()); break;   
            }
        }
    }

    IEnumerator MoveToBoss()
    {
        Inventory playerInv = _player.GetComponent<Inventory>();

        if (playerInv.playerItems.Any(item => item.id == InventoryTypes.Key))
        {
            playerInv.RemoveItem(InventoryTypes.Key);
        }

        _fade.SetActive(true);
        yield return new WaitForSeconds(1);
        GameManager.Instance.MoveToFinalBoss();
    }

    IEnumerator MoveToFinal()
    {
        _fade.SetActive(true);
        yield return new WaitForSeconds(1);
    }
}
