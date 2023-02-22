using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DAnimator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private Transform _playerDirection;
    private Player3D _player;

    public bool IsSprinting { get; set; }

    enum LookDirection
    {
        Left,
        Right,
        Front,
        Back,
        LB,
        RB,
        RF,
        LF
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player3D>();

        _player.LookDirection = (int)LookDirection.Back;
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            _animator.SetBool("Walk", true);
        }
        if(horizontalInput == 0 && verticalInput == 0)
        {
            _animator.SetBool("Walk", false);
        }

        CheckLookDirection(horizontalInput, verticalInput);
        CheckForSprint();
    }

    public void Jump()
    {
        _animator.SetTrigger("Jump");
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

    void CheckLookDirection(float horizontalMove, float verticalMove)
    {
        if (horizontalMove > 0) // moves right
        {
            //_playerDirection.transform.rotation = Quaternion.Euler(-90f, 0f, 90f);
            _player.LookDirection = (int)LookDirection.Right;
        }
        if (horizontalMove < 0) // moves left
        {
            _player.LookDirection = (int)LookDirection.Left;
        }
        if (verticalMove < 0) //goes down
        {
            _player.LookDirection = (int)LookDirection.Front;
        }
        if (verticalMove > 0) //goes up
        {
            _player.LookDirection = (int)LookDirection.Back;
        }
        if (horizontalMove == -1 && verticalMove == 1) // LB
        {
            _player.LookDirection = (int)LookDirection.LB;
        }
        if (horizontalMove == 1 && verticalMove == 1) // RB
        {
            _player.LookDirection = (int)LookDirection.RB;
        }
        if (horizontalMove == -1 && verticalMove == -1) // LF
        {
            _player.LookDirection = (int)LookDirection.LF;
        }
        if (horizontalMove == 1 && verticalMove == -1) // RF
        {
            _player.LookDirection = (int)LookDirection.RF;
        }
    }

    private void DebugLookDirection()
    {
        switch (_player.LookDirection)
        {
            case (int)LookDirection.Left: Debug.Log("Moving left"); break;
            case (int)LookDirection.Right: Debug.Log("Moving right"); break;
            case (int)LookDirection.Back: Debug.Log("Moving up"); break;
            case (int)LookDirection.Front: Debug.Log("Moving down"); break;
            case (int)LookDirection.LB: Debug.Log("Moving LB"); break;
            case (int)LookDirection.RB: Debug.Log("Moving RB"); break;
            case (int)LookDirection.RF: Debug.Log("Moving RF"); break;
            case (int)LookDirection.LF: Debug.Log("Moving LF"); break;
        }
    }
}
