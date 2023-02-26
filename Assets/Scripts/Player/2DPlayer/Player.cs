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
    private int _ammoAmount;
    private double _ammoToBullets = 15.0;

    private PlayerAnimator _animator;
    private WeaponGraphicController[] _weaponGraphicControllers;
    private WeaponGraphicController _currentWeaponGraphic;
    private Rigidbody2D _rigidbody;
    private Inventory _playerInventory;
    [SerializeField]
    protected GameObject damageReceivedTextPrefab;
    [SerializeField]
    private PlayerShield _shield;

    public float speed = 3.0f;
    [SerializeField]
    private float _currentSpeed;

    public int LookDirection { get; set; }
    public int Health { get; set; }

    private float _canFire = 0.0f;
    private float _fireRate = 1.0f;
    private bool _hasFired;

    public bool FireBlocked { get; set; }
    public bool IsShieldEnabled { get; private set; }
    private bool _hasShield;
    [SerializeField]
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
        _weaponGraphicControllers = GetComponentsInChildren<WeaponGraphicController>();
        GetInventory();

        var ammoBoxCount = _playerInventory.playerItems.Count(item => item.id == InventoryTypes.Ammo);
        _ammoAmount = ammoBoxCount * (int)_ammoToBullets;

        Health = 4;
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
    }

    public void SaveInventory()
    {
        StringBuilder stringOfId = new StringBuilder();

        foreach (var item in _playerInventory.playerItems)
        {
            stringOfId.Append(item.id);
            stringOfId.Append(" ");
        }

        PlayerPrefs.SetString("Inventory", stringOfId.ToString());
        PlayerPrefs.Save();
    }

    void ChooseWeaponGraphic()
    {
        if (_playerInventory.SelectedItem != null)
        {
            _currentWeaponGraphic = _weaponGraphicControllers.SingleOrDefault(controller => controller.InventoryId == _playerInventory.SelectedItem.id);
            if (_currentWeaponGraphic != null)
            {
                _currentWeaponGraphic.SwitchWeaponPosition(LookDirection);
            }
        }
    }

    void HideWeaponGraphic()
    {
        if (_currentWeaponGraphic != null)
        {
            _currentWeaponGraphic.HideGraphics();
        }
    }

    void Update()
    {
        if (isDead) return;
        if (GameManager.Instance.IsPaused) return;

        CalculateMovement();
        UpdateSound();
        CheckShield();
        if (_playerInventory.SelectedItem != null)
        {
            CheckFire();
        }

        _currentSpeed = speed;
    }

    public void AddToAmmo()
    {
        _ammoAmount += (int)_ammoToBullets;
    }

    public void UpdateAmmo(int ammount)
    {
        _ammoAmount = ammount;
        Debug.Log("AMMO AMOUNT: " + _ammoAmount);

        var currentBoxexCount = _ammoAmount / _ammoToBullets;
        var ammoBoxesInInventory = _playerInventory.playerItems.Count(item => item.id == InventoryTypes.Ammo);
        if (currentBoxexCount <= ammoBoxesInInventory - 1)
        {
            _playerInventory.RemoveItem(InventoryTypes.Ammo);
        }
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
            ChooseWeaponGraphic();
            if (_currentWeaponGraphic != null)
            {
                FindActiveWeapon((int)_ammoAmount);
            }

            _canFire = Time.time + _fireRate;
            _hasFired = true;
        }
        if (Input.GetMouseButtonUp(0) && _hasFired)
        {
            speed = _isSprinting ? 3.0f * 2 : 3.0f;
            HideWeaponGraphic();
            _animator.CeaseFire();
            _hasFired = false;
        }
    }

    void FindActiveWeapon(int ammoAmount) // when calling this method item selected must be a weapon
    {
        switch (_playerInventory.SelectedItem.id)
        {
            case InventoryTypes.Rifle:
                var activeRifle = _currentWeaponGraphic.GetComponentsInChildren<Rifle>().Single(child => child.isActiveAndEnabled);
                activeRifle.Fire(LookDirection, ammoAmount);
                break;
            case InventoryTypes.Pistol:
                var activePistol = _currentWeaponGraphic.GetComponentsInChildren<Pistol>().Single(child => child.isActiveAndEnabled);
                activePistol.Fire(LookDirection, ammoAmount);
                break;
            default: break;
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

        _rigidbody.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);
        _animator.Move(horizontalInput, verticalInput);
    }

    private void ShowFloatingDamage(int damage, Color color)
    {
        GameObject damageText = Instantiate(damageReceivedTextPrefab, transform.position, Quaternion.identity) as GameObject;
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
                OnPlayerDeath();
            }
            else
            {
                StartCoroutine(StopOnHit());
            }
        }
    }

    public void OnPlayerDeath()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.playerDeath, transform.position);

        _rigidbody.velocity = Vector3.zero;
        speed = 0f;
        _animator.OnPlayerDeath();
        isDead = true;
        GameManager.Instance.IsPlayerDead = true;
        Destroy(gameObject, 2f);
    }

    IEnumerator StopOnHit()
    {
        float localSpeed = _currentSpeed;

        _sprintAllowed = false;
        speed = 0;
        yield return new WaitForSeconds(0.3f);
        speed = localSpeed;
        _sprintAllowed = true;
    }

    public void UpdateSound()
    {
        if ((_rigidbody.velocity.x != 0 || _rigidbody.velocity.y != 0) && !_isSprinting)
        {
            _playerFootstepsSprint.stop(STOP_MODE.IMMEDIATE);

            PLAYBACK_STATE playbackState;
            _playerFootstepsWalk.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _playerFootstepsWalk.start();
            }
        }
        else if ((_rigidbody.velocity.x != 0 || _rigidbody.velocity.y != 0) && _isSprinting)
        {
            _playerFootstepsWalk.stop(STOP_MODE.IMMEDIATE);

            PLAYBACK_STATE playbackState;
            _playerFootstepsSprint.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _playerFootstepsSprint.start();
            }
        }
        else if (_rigidbody.velocity.x == 0 && _rigidbody.velocity.y == 0)
        {
            _playerFootstepsWalk.stop(STOP_MODE.ALLOWFADEOUT);
            _playerFootstepsSprint.stop(STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            
        }
    }
}
