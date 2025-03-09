using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class Player : MonoBehaviour, IDamageable
{
    public bool isDead;
    public int LookDirection { get; set; }
    public int Health { get; set; }
    public float speed = 3.0f;

    [SerializeField] private PlayerWeaponController _playerWeaponController;
    [SerializeField] private GameObject _damageReceivedTextPrefab;
    [SerializeField] private PlayerShield _shield;

    private const double AmmoToBulletsRate = 15.0;
    private const float InitialSpeed = 3.0f;

    private PlayerAnimator _animator;
    private Rigidbody2D _rigidbody;
    private Inventory _playerInventory;

    private int _ammoAmount;
    private float _canFire = 0.0f;
    private float _fireRate = 1.0f;

    private bool _hasFired;
    public bool FireBlocked { get; set; }
    public bool IsShieldEnabled { get; set; }
    private bool _hasShield;
    private bool _isSprinting;
    private bool _sprintAllowed;

    private EventInstance _playerFootstepsWalk;
    private EventInstance _playerFootstepsSprint;

    void Start()
    {
        _playerFootstepsWalk = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.playerFootstepsWalk);
        _playerFootstepsSprint = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.playerFootstepsSprint);

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<PlayerAnimator>();
        _playerInventory = GetComponent<Inventory>();

        GetInventory();

        var ammoBoxCount = _playerInventory.playerItems.Count(item => item.id == InventoryTypes.Ammo);
        _ammoAmount = ammoBoxCount * (int)AmmoToBulletsRate;
        UIManager.Instance.UpdateAmmoCount(_ammoAmount);

        Health = PlayerSettings.Health;
        _sprintAllowed = true;
    }

    public void HasShield(bool state)
    {
        _hasShield = state;
        _shield.gameObject.SetActive(true);
    }

    private void GetInventory()
    {
        string inventoryIdString = PlayerPrefs.GetString(PlayerSettings.Inventory, string.Empty);

        if (!string.IsNullOrEmpty(inventoryIdString))
        {
            List<int> indexes = new List<int>();
            var IdToAdd = inventoryIdString.Split(null);

            foreach (var id in IdToAdd)
            {
                if (!string.IsNullOrEmpty(id))
                    indexes.Add(int.Parse(id));
            }
            for (int i = 0; i < indexes.Count; i++)
            {
                _playerInventory.GiveItem(indexes[i]);
            }
        }

        _hasShield = _playerInventory.playerItems.Any(item => item.id == InventoryTypes.Shield)
            || _playerInventory.playerItems.Any(item => item.id == InventoryTypes.EnhancedShield) ?
            true : false;
    }

    public void SaveInventory()
    {
        StringBuilder stringOfId = new StringBuilder();

        foreach (var item in _playerInventory.playerItems)
        {
            stringOfId.Append(item.id);
            stringOfId.Append(" ");
        }

        PlayerPrefs.SetString(PlayerSettings.Inventory, stringOfId.ToString());
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (isDead) return;
        if (GameManager.Instance.IsPaused)
        {
            IsShieldEnabled = false;
            _shield.ShowShield(false);
            return;
        }

        CalculateMovement();
        UpdateSound();
        CheckShield();
        if (_playerInventory.SelectedItem != null)
        {
            CheckFire();
        }
    }

    public void AddToAmmo()
    {
        _ammoAmount += (int)AmmoToBulletsRate;
        UIManager.Instance.UpdateAmmoCount(_ammoAmount);
    }

    public void UpdateAmmo(int ammount)
    {
        _ammoAmount = ammount;

        var currentBoxexCount = _ammoAmount / AmmoToBulletsRate;
        var ammoBoxesInInventory = _playerInventory.playerItems.Count(item => item.id == InventoryTypes.Ammo);
        if (currentBoxexCount <= ammoBoxesInInventory - 1)
        {
            _playerInventory.RemoveItem(InventoryTypes.Ammo);
        }

        UIManager.Instance.UpdateAmmoCount(_ammoAmount);
    }

    void CheckShield()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _hasShield && _shield.CanDelflect)
        {
            IsShieldEnabled = true;
            _shield.ShowShield(true);
        }
        if (Input.GetKeyUp(KeyCode.Space) && IsShieldEnabled)
        {
            IsShieldEnabled = false;
            _shield.ShowShield(false);
        }
    }

    void CheckFire()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > _canFire && _ammoAmount > 0 && !FireBlocked)
        {
            speed = 0;
            _animator.Fire();

            _playerWeaponController.OnShoot(_ammoAmount);

            _canFire = Time.time + _fireRate;
            _hasFired = true;
        }
        if (Input.GetMouseButtonUp(0) && _hasFired)
        {
            _playerWeaponController.HideWeapon();

            speed = _isSprinting ? InitialSpeed * 2 : InitialSpeed;
            _animator.CeaseFire();
            _hasFired = false;
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && _sprintAllowed)
        {
            speed *= 2;
            _animator.IsSprinting = true;
            _isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isSprinting)
        {
            speed /= 2;
            _animator.IsSprinting = false;
            _isSprinting = false;
        }

        _rigidbody.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        _animator.Move(horizontalInput, verticalInput);
    }

    private void ShowFloatingDamage(int damage, Color color)
    {
        GameObject damageText = Instantiate(_damageReceivedTextPrefab, transform.position, Quaternion.identity) as GameObject;
        damageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        damageText.GetComponent<TextMeshProUGUI>().color = color;
        damageText.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Image>().transform);
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        if (IsShieldEnabled)
        {
            ShowFloatingDamage(damage, Color.green);
            _shield.OnDefend();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shieldDeflect, transform.position);
        }
        else
        {
            ShowFloatingDamage(damage, Color.red);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hit, this.transform.position);

            Health -= damage;
            _animator.OnPlayerHit(LookDirection);
            UIManager.Instance.UpdateHealthUI(Health);

            if (Health <= 0)
            {
                if (_playerInventory.playerItems.Any(item => item.id == InventoryTypes.Potion))
                {
                    PotionHeal();
                }
                else
                {
                    OnPlayerDeath();
                }
            }
        }
    }

    public void PotionHeal()
    {
        Health = PlayerSettings.Health;
        UIManager.Instance.UpdateHealthUI(Health);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.potionHeal, this.transform.position);
        _playerInventory.RemoveItem(InventoryTypes.Potion);
        UIManager.Instance.DisplayMessage("Potion used to restore health!");
    }

    public void OnPlayerDeath()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.playerDeath, transform.position);

        _rigidbody.linearVelocity = Vector3.zero;
        speed = 0f;
        _animator.OnPlayerDeath();
        isDead = true;
        GameManager.Instance.IsPlayerDead = true;
        Destroy(gameObject, 2f);
    }

    public void UpdateSound()
    {
        if ((_rigidbody.linearVelocity.x != 0 || _rigidbody.linearVelocity.y != 0) && !_isSprinting)
        {
            _playerFootstepsSprint.stop(STOP_MODE.IMMEDIATE);

            PLAYBACK_STATE playbackState;
            _playerFootstepsWalk.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _playerFootstepsWalk.start();
            }
        }
        else if ((_rigidbody.linearVelocity.x != 0 || _rigidbody.linearVelocity.y != 0) && _isSprinting)
        {
            _playerFootstepsWalk.stop(STOP_MODE.IMMEDIATE);

            PLAYBACK_STATE playbackState;
            _playerFootstepsSprint.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _playerFootstepsSprint.start();
            }
        }
        else if (_rigidbody.linearVelocity.x == 0 && _rigidbody.linearVelocity.y == 0)
        {
            _playerFootstepsWalk.stop(STOP_MODE.ALLOWFADEOUT);
            _playerFootstepsSprint.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
