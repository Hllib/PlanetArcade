using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour, IGatherable
{
    public Inventory PlayerInventory { get; set; }
    [SerializeField]
    private int _inventoryType;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInventory = other.GetComponent<Inventory>();
            PlayerInventory.GiveItem(_inventoryType);
            Destroy(gameObject);
        }
    }
}
