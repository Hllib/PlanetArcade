using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField]
    private Player _player;

    public override void Init()
    {
        this.inventoryType = InventoryTypes.Pistol;
    }

    IEnumerator FireRoutine(int ammoAmount)
    {
        while (true)
        {
            if (ammoAmount <= 0)
            {
                StopAllCoroutines();
                break;
            }
            //raycast
            ammoAmount -= 1;
            _player.UpdateAmmo(ammoAmount);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.pistolFire, this.transform.position);

            yield return new WaitForSeconds(1f);
        }
    }

}
