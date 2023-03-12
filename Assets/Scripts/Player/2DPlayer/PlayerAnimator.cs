using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Player _player;

    public bool IsSprinting { get; set; }

    enum LookDirection
    {
        Left,
        Right,
        Front,
        Back
    }

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.Log("_spriteRenderer is NULL::PlayerAnimator.cs");
        }

        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            Debug.Log("_lampAnimator is NULL::PlayerAnimator.cs");
        }

        _player = GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("_player is NULL::PlayerAnimator.cs");
        }

        _player.LookDirection = (int)LookDirection.Front;
    }

    public void Move(float horizontalMove, float verticalMove)
    {
        FlipSpriteH(horizontalMove);
        _animator.SetFloat("MoveHorizontal", Mathf.Abs(horizontalMove));
        _animator.SetFloat("MoveVertical", verticalMove);
        CheckDiagonalMovement(Mathf.Abs(horizontalMove));
        CheckForSprint();

        CheckVerticalMovement(verticalMove);
    }

    private void CheckVerticalMovement(float verticalMove)
    {
        if (verticalMove < 0) //goes down
        {
            _player.LookDirection = (int)LookDirection.Front;
        }
        if (verticalMove > 0) //goes up
        {
            _player.LookDirection = (int)LookDirection.Back;
        }
    }

    private void CheckDiagonalMovement(float horizonatalMove)
    {
        if (horizonatalMove > 0)
        {
            _animator.SetBool("Diagonal", true);
        }
        if (horizonatalMove == 0)
        {
            _animator.SetBool("Diagonal", false);
        }
    }

    private void CheckForSprint()
    {
        if (IsSprinting)
        {
            _animator.SetBool("Sprint", true);
        }
        if (!IsSprinting)
        {
            _animator.SetBool("Sprint", false);
        }
    }

    private void FlipSpriteH(float horizontalMove)
    {
        if (horizontalMove > 0) // moves right
        {
            _spriteRenderer.flipX = true;
            _player.LookDirection = (int)LookDirection.Right;
        }
        if (horizontalMove < 0) // moves left
        {
            _spriteRenderer.flipX = false;
            _player.LookDirection = (int)LookDirection.Left;
        }
    }

    public void Fire()
    {
        _animator.SetBool("Fire", true);
    }

    public void CeaseFire()
    {
        _animator.SetBool("Fire", false);
    }

    public void OnPlayerHit(int lookDirection)
    {
        var directionToPass = 0;
        switch (lookDirection)
        {
            case (int)LookDirection.Front: directionToPass = 0; break;
            case (int)LookDirection.Back: directionToPass = 1; break;
            default: directionToPass = 2; break;
        }

        _animator.SetInteger("LookDirection", directionToPass);
        _animator.SetTrigger("Hit");
    }

    public void ChooseShootDirection(int lookDirection)
    {
        var directionToPass = 0;
        switch (lookDirection)
        {
            case (int)LookDirection.Front: directionToPass = 0; break;
            case (int)LookDirection.Back: directionToPass = 1; break;
            case (int)LookDirection.Left: directionToPass = 2; _spriteRenderer.flipX = false; break;
            case (int)LookDirection.Right: directionToPass = 2; _spriteRenderer.flipX = true; break;
        }

        _animator.SetInteger("LookDirection", directionToPass);
    }

    public void OnPlayerDeath()
    {
        _animator.SetTrigger("IsDead");
    }
}
