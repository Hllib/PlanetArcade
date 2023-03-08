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

    public void OnShoot()
    {
        _weaponScriptableObject = _weaponHolderScriptableObject.weapons.FirstOrDefault(weapon
            => weapon.title == _playerInventory.SelectedItem.title);

        if (_weaponScriptableObject != null)
        {
            _weaponSpriteRenderer.sprite = _weaponScriptableObject.sprite;
            _weaponSpriteRenderer.enabled = true;

            _weaponScriptableObject.Shoot();
        }
    }

    public void HideWeapon()
    {
        _weaponSpriteRenderer.enabled = false;
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
}
