using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGatherable
{
    public Inventory PlayerInventory { get; set; }
    public void OnTriggerEnter2D(Collider2D other);
}
