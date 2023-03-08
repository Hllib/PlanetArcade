using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Transform _aimTransform;
    private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField]
    private Inventory _playerInventory;
    [SerializeField]
    private Player _player;

    [SerializeField]
    private WeaponHolderScriptableObject _weaponHolderScriptableObject;
    private WeaponScriptableObject _weaponScriptableObject;

    private void Start()
    {
        _aimTransform = GetComponent<Transform>();
        _weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>(false);
    }

    private void Update()
    {
        Aim();
    }

    public void OnShoot(int ammoAmount)
    {
        _weaponScriptableObject = _weaponHolderScriptableObject.weapons.FirstOrDefault(weapon
            => weapon.title == _playerInventory.SelectedItem.title);

        if (_weaponScriptableObject != null)
        {
            _weaponSpriteRenderer.sprite = _weaponScriptableObject.sprite;
            _weaponSpriteRenderer.transform.localScale = new Vector3(_weaponScriptableObject.scaleFactor, _weaponScriptableObject.scaleFactor, 0);
            _weaponSpriteRenderer.enabled = true;

            StartCoroutine(ShootRoutine(ammoAmount)); 
        }
    }

    public void HideWeapon()
    {
        _weaponSpriteRenderer.enabled = false;
        StopAllCoroutines();
    }

    private void Aim()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        //Vector2 worldPosition2D = new Vector2(worldPosition.x, worldPosition.y);

        Vector3 aimDirection = (worldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        _aimTransform.localScale = aimLocalScale;
    }

    IEnumerator ShootRoutine(int ammoAmount)
    {
        while (true)
        {
            if (ammoAmount <= 0)
            {
                StopAllCoroutines();
                break;
            }

            _weaponScriptableObject.Shoot();
            ammoAmount -= 1;
            _player.UpdateAmmo(ammoAmount);

            yield return new WaitForSeconds(1f);
        }
    }
}
