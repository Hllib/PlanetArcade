using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _scopeSpriteRenderer;
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private Player _player;

    private Transform _aimTransform;
    private SpriteRenderer _weaponSpriteRenderer;
    private PlayerAnimator _playerAnimator;
    private int _lookDirection;

    [SerializeField] private WeaponHolderScriptableObject _weaponHolderScriptableObject;
    private WeaponScriptableObject _weaponScriptableObject;

    [SerializeField]
    private Transform _gunPoint;

    private int _playerSpriteOrderInLayer = 50;
    float _playerLookAngle;
    private bool _isShooting = false;

    enum LookDirection
    {
        Left,
        Right,
        Front,
        Back
    }

    private void Start()
    {
        _aimTransform = GetComponent<Transform>();
        _weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>(false);
        _playerAnimator = _player.GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        CheckPlayerLookDirection(_playerLookAngle);
        Aim();
    }

    public void OnShoot(int ammoAmount)
    {
        _weaponScriptableObject = _weaponHolderScriptableObject.weapons.FirstOrDefault(weapon
            => weapon.title == _playerInventory.SelectedItem.title);

        if (_weaponScriptableObject != null)
        {
            _isShooting = true;

            _gunPoint.transform.localPosition = new Vector3(_weaponScriptableObject.shootStartPoints.X, _weaponScriptableObject.shootStartPoints.Y, 0);
            _weaponSpriteRenderer.sprite = _weaponScriptableObject.sprite;
            _weaponSpriteRenderer.transform.localScale = new Vector3(_weaponScriptableObject.scaleFactor, _weaponScriptableObject.scaleFactor, 0);

            _weaponSpriteRenderer.enabled = true;
            _scopeSpriteRenderer.enabled = true;

            _playerAnimator.GetComponentInChildren<Animator>().SetBool("Fire", true);
            StartCoroutine(ShootRoutine(ammoAmount));
        }
    }

    public void HideWeapon()
    {
        _isShooting = false;
        _playerAnimator.GetComponentInChildren<Animator>().SetBool("Fire", false);
        _weaponSpriteRenderer.enabled = false;
        StopAllCoroutines();
        _scopeSpriteRenderer.enabled = false;
    }

    private void CheckPlayerLookDirection(float angle)
    {
        _weaponSpriteRenderer.sortingOrder = _playerSpriteOrderInLayer + 1;

        if (angle <= 70 && angle >= -70)
        {
            _lookDirection = (int)LookDirection.Right;
        }
        else if (angle > 70 && angle < 110)
        {
            _lookDirection = (int)LookDirection.Back;
            _weaponSpriteRenderer.sortingOrder = _playerSpriteOrderInLayer - 1;
        }
        else if ((angle >= 110 && angle <= 180) || (angle > -180 && angle < -110))
        {
            _lookDirection = (int)LookDirection.Left;
        }
        else if (angle < -70 && angle > -110)
        {
            _lookDirection = (int)LookDirection.Front;
        }
    }

    private void Aim()
    {
         Vector3 mousePos = Input.mousePosition;
         mousePos.z = Camera.main.nearClipPlane;
         Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
         Vector3 aimDirection = (worldPosition - transform.position).normalized;
         _playerLookAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
         _aimTransform.eulerAngles = new Vector3(0, 0, _playerLookAngle);

        Vector3 aimLocalScale = Vector3.one;
        if (_playerLookAngle > 90 || _playerLookAngle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        _aimTransform.localScale = aimLocalScale;

        if (_isShooting)
        {
            _playerAnimator.ChooseShootDirection(_lookDirection);
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = ObjectPooler.Instance.GetPooledObject();

        if(bullet != null )
        {
            bullet.transform.position = _gunPoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<Bullet>().GetMessage(_weaponScriptableObject.bulletScriptableObject);
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().AddForce(_weaponScriptableObject.fireForce * _gunPoint.transform.right, ForceMode2D.Impulse);
            AudioManager.Instance.PlayOneShot(_weaponScriptableObject.shootSound, this.transform.position);
        }
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

            ShootBullet();
            ammoAmount -= 1;
            _player.UpdateAmmo(ammoAmount);

            yield return new WaitForSeconds(_weaponScriptableObject.fireRateDelay);
        }
    }
}
