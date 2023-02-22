using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private int _health = 2;
    [SerializeField]
    private int _deflectTolerance;
    private float _coolDownTime = 7.0f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public bool CanDelflect { get; private set; }

    private void Start()
    {
        _deflectTolerance = _health;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        CanDelflect= true;
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
        _spriteRenderer.enabled = state;
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_coolDownTime);
        _deflectTolerance = _health;
        CanDelflect = true;
    }
}
