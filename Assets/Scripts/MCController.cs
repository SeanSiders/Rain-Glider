using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MCController : MonoBehaviour
{
    public float topSpeed = 1f;
    public float acceleration = 0.2f;
    public Transform player;
    public float playerRotationSpeed = 5f;

    private Vector3 velocity;
   
    private PlayerInputActions _playerInputActions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private LayerMask _groundLayerMask;
    private bool _isGrounded;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        _moveAction = _playerInputActions.Player.Move;
        _moveAction.Enable();

        _lookAction = _playerInputActions.Player.Look;
        _lookAction.Enable();

        _playerInputActions.Player.Fire.Enable();

        _playerInputActions.Player.Jump.performed += OnJump;
        _playerInputActions.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();

        _playerInputActions.Player.Fire.Disable();

        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            Debug.Log("JUMP");
            velocity.y = 1.0f;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 toGround = 2.0f * Vector3.down;

        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, toGround.magnitude, _groundLayerMask);

        if (_isGrounded)
        {
            if (velocity.y < 0) velocity.y = 0;
            transform.position = hit.point - 0.8f * toGround;
        }
        else
        {
            velocity += 0.05f * Vector3.down;
        }

        InputMovement();
        RotateTowardsMovement();

        transform.Translate(velocity, Space.World);
    }
    private void InputMovement()
    {
        Vector2 moveDirection = _moveAction.ReadValue<Vector2>();

        // Get the direction that the player is trying to move in
        Vector3 components = moveDirection.x * Camera.main.transform.right + moveDirection.y * Camera.main.transform.forward;
        components.y = 0.0f;

        // Treat no input as "STOP". Otherwise move in that direction.
        if (moveDirection.magnitude <= 0.0)
        {
            velocity -= 0.1f * velocity;
        }
        else
        {
            velocity += acceleration * components;

            Vector3 flatMotion = Vector3.ClampMagnitude(new(velocity.x, 0.0f, velocity.z), topSpeed);
            velocity.x = flatMotion.x;
            velocity.z = flatMotion.z;
        }
    }

    private void RotateTowardsMovement()
    {
        Vector3 lookDirection = new(velocity.x, 0.0f, velocity.z);
        if (lookDirection.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), playerRotationSpeed);
        }
    }
}
