using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private int _health = 2;
    private int _healthEnhanced = 5;
    [SerializeField]
    private int _deflectTolerance;

    private float _coolDownTime = 7.0f;
    private float _coolDownTimeEnhanced = 5.0f;
    private float _coolDown;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Sprite _regularShield;
    [SerializeField]
    private Sprite _enhancedShield;

    public bool CanDelflect { get; private set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        CanDelflect = true;
    }

    private void ChooseShield()
    {
        var playerInv = _player.GetComponent<Inventory>();

        if (playerInv.playerItems.Any(item => item.id == InventoryTypes.EnhancedShield))
        {
            _spriteRenderer.sprite = _enhancedShield;
            _deflectTolerance = _healthEnhanced;
            _coolDown = _coolDownTimeEnhanced;
        }
        else 
        {
            _spriteRenderer.sprite = _regularShield;    
            _deflectTolerance = _health;
            _coolDown = _coolDownTime;
        }
    }

    public void OnDefend()
    {
        _deflectTolerance -= 1;

        if (_deflectTolerance > 0)
        {
            _animator.SetTrigger("Defend");
        }
        else 
        {
            CanDelflect = false;
            _spriteRenderer.enabled = false;
            StartCoroutine(CoolDown());
        }
    }

    public void ShowShield(bool state)
    {
        ChooseShield();
        _spriteRenderer.enabled = state;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_coolDown);
        _deflectTolerance = _health;
        CanDelflect = true;
    }
}
