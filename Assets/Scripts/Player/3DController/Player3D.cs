using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;
    private Player3DAnimator _animator;
    public EventInstance playerFootsteps;

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

    public Inventory playerInventory;

    public int goldAmount;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Player3DAnimator>();
        playerInventory = GetComponent<Inventory>();

        GetInventory();

        for (int i = 0; i < playerInventory.playerItems.Count; i++)
        {
            if (playerInventory.playerItems[i].id == InventoryTypes.Gold)
            {
                goldAmount += 1;
            }
        }

        playerFootsteps = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.footsteps);
    }

    void Update()
    {
        if (DialogueManager.Instance.IsDialogueDisplayed || BlockMovement)
        {
            return;
        }

        MovePlayer();
        MoveCamera();
        UpdateSound();

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
                playerInventory.GiveItem(indexes[i]);
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

        _characterController.Move(movement * _speed * Time.deltaTime);
        _characterController.Move(_velocity * Time.deltaTime);
        _animator.Move(horizontalInput, verticalInput);
    }

    public void UpdateSound()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && _characterController.isGrounded)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else if ((Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) || !_characterController.isGrounded)
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
