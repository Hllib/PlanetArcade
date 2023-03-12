using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IGatherable
{
    public Inventory PlayerInventory { get; set; }
    protected int inventoryType;

    public virtual void Init()
    {

    }

    public virtual void Start()
    {
        Init();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory = other.GetComponent<Inventory>();

            PlayerInventory.GiveItem(inventoryType);
            Destroy(gameObject);
        }
    }
}
