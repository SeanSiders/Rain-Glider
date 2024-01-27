using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MCController : MonoBehaviour
{
   public Transform groundCollider;
   private PlayerInputActions _playerInputActions;
   private InputAction _moveAction;
   private InputAction _lookAction;
   private Rigidbody _rigidbody;
   
   public float speed = 40f;
   public float jumpForce = 10f;

   private LayerMask _groundLayerMask;

   private bool _isGrounded;

   private void Awake()
   {
      _playerInputActions = new PlayerInputActions();
      _rigidbody = GetComponent<Rigidbody>();

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
         _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
      _isGrounded = Physics.Raycast(groundCollider.position, Vector3.down, 0.05f, _groundLayerMask); 

      Vector2 moveDirection = _moveAction.ReadValue<Vector2>();
      Vector3 velocity = _rigidbody.velocity;

      velocity.x = speed * moveDirection.x;
      velocity.z = speed * moveDirection.y;

      _rigidbody.velocity = velocity;

      Vector2 lookDirection = _lookAction.ReadValue<Vector2>();
    }
}
