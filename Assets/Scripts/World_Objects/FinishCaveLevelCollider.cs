using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishCaveLevelCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory _playerInv = collision.GetComponent<Inventory>();

            if (_playerInv != null)
            {
                if (_playerInv.playerItems.Any(item => item.id == InventoryTypes.Key))
                {
                    Guide.Instance.CheckLevelFinish_Cave(true);
                }
                else
                {
                    Guide.Instance.CheckLevelFinish_Cave(false);
                }
            }
        }

    }
}
