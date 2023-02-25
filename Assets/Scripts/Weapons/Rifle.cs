using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class Rifle : Weapon
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Player _player;

    public override void Init()
    {
        base.Init();

        this.inventoryType = InventoryTypes.Rifle;
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
            if(ammoAmount <= 0)
            {
                break;
            }
            Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(0f, bulletRotationY, bulletRotationZ));
            ammoAmount -= 1;
            _player.UpdateAmmo(ammoAmount);
            UIManager.Instance.DisplayMessage($"Ammo left: {ammoAmount}");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.rifleFire, this.transform.position);

            yield return new WaitForSeconds(1f);
        }
    }
}
