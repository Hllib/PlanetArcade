using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ammo : MonoBehaviour, IGatherable
{
    public Inventory PlayerInventory { get; set; }

    private void Start()
    {
        PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory.GiveItem(InventoryTypes.Ammo);
            Player player = other.GetComponent<Player>();
            player.AddToAmmo();
            Destroy(gameObject);
        }
    }
}
