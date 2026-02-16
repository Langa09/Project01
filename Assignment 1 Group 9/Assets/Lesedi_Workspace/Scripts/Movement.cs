using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("References")]
    public CartController cartController; // Drag your Cart here

    private Rigidbody2D rb;
    private Controls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.PlayerControls.Move.performed += OnMove;
        controls.PlayerControls.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        controls.PlayerControls.Move.performed -= OnMove;
        controls.PlayerControls.Move.canceled -= OnMove;
        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Move player
        rb.linearVelocity = moveInput * moveSpeed;

        // Rotate player to face movement
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);

            // Inform cart of player facing
            if (cartController != null)
                cartController.UpdatePlayerDirection(transform.up);
        }
    }
}
