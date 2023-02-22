using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IGatherable
{
    public Inventory PlayerInventory { get; set; }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInventory = other.GetComponent<Inventory>();
            PlayerInventory.GiveItem(InventoryTypes.Key);
            Destroy(gameObject);
        }
    }
}
