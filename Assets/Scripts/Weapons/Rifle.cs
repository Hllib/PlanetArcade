using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public override void Init()
    {
        this.inventoryType = InventoryTypes.Rifle;
    }
}
