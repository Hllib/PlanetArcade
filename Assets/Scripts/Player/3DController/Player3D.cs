using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;
    private Player3DAnimator _animator;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpforce;
    [SerializeField]
    private float _gravity = -9.81f;

    private Vector3 _velocity;
    private Vector3 _movementInput;

    private Vector2 _mouseInput;
    private float _rotationX;
    [SerializeField]
    private float _sensitivity;
    [SerializeField]
    private Transform _camera;

    public int LookDirection { get; set; }
    public bool HasInteracted { get; set; }
    public bool BlockMovement { get; set; }

    public Inventory _playerInventory;

    public int goldAmount;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Player3DAnimator>();
        _playerInventory = GetComponent<Inventory>();

        GetInventory();

        for (int i = 0; i < _playerInventory.playerItems.Count; i++)
        {
            if (_playerInventory.playerItems[i].id == InventoryTypes.Gold)
            {
                goldAmount += 1;
            }
        }
    }

    void Update()
    {
        if (DialogueManager.Instance.IsDialogueDisplayed || BlockMovement)
        {
            return;
        }

        MovePlayer();
        MoveCamera();

        if (Input.GetKeyDown(KeyCode.E))
        {
            HasInteracted = true;
        }
    }

    private void GetInventory()
    {
        string inventoryIdString = PlayerPrefs.GetString("Inventory", string.Empty);

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

    private void MoveCamera()
    {
        _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _rotationX -= _movementInput.y * _sensitivity;
        transform.Rotate(0, _mouseInput.x * _sensitivity, 0);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        _movementInput = new Vector3(horizontalInput, 0f, verticalInput);

        Vector3 movement = transform.TransformDirection(_movementInput);

        if (_characterController.isGrounded)
        {
            _velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = _jumpforce;
                _animator.Jump();
            }
        }
        else
        {
            _velocity.y -= _gravity * -2f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= 2;
            _animator.IsSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= 2;
            _animator.IsSprinting = false;
        }

        _characterController.Move(movement * _speed * Time.deltaTime);
        _characterController.Move(_velocity * Time.deltaTime);
        _animator.Move(horizontalInput, verticalInput);
    }

}
