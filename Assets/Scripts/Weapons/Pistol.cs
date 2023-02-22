using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Player _player;

    public override void Init()
    {
        base.Init();

        this.inventoryType = InventoryTypes.Pistol;
    }

    public override void Fire(int fireDirection, int ammoAmount)
    {
        base.Fire(fireDirection, ammoAmount);

        _player = transform.GetComponentInParent<Player>();
        StartCoroutine(FireRoutine(ammoAmount));
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
            Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(0f, bulletRotationY, bulletRotationZ));
            ammoAmount -= 1;
            _player.UpdateAmmo(ammoAmount);
            UIManager.Instance.DisplayMessage($"Ammo left: {ammoAmount}");
            yield return new WaitForSeconds(1f);
        }
    }

}
