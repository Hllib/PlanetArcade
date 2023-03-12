using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Init()
    {
        this.inventoryType = InventoryTypes.Pistol;
    }
}
