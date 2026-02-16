using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CartController : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Drag Player here

    [Header("Cart Settings")]
    public float distanceFromPlayer = 1.2f;
    public float maxAngleOffset = 35f;
    public float rotationSpeed = 360f;
    public float stickDeadzone = 0.2f;

    private Rigidbody2D rb;
    private Controls controls; // ⚡ Initialize in Awake
    private Vector2 aimInput;
    private Vector2 playerForward = Vector2.up;
    private float currentOffset = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Cart manual control
        controls = new Controls(); // ✅ Safe initialization
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.PlayerControls.CartAim.performed += OnCartAim;
        controls.PlayerControls.CartAim.canceled += OnCartAim;
    }

    private void OnDisable()
    {
        controls.PlayerControls.CartAim.performed -= OnCartAim;
        controls.PlayerControls.CartAim.canceled -= OnCartAim;
        controls.Disable();
    }

    private void OnCartAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
    }

    // Called by player to update facing
    public void UpdatePlayerDirection(Vector2 forward)
    {
        playerForward = forward.normalized;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Right stick X only
        float horizontal = aimInput.x;

        // Apply deadzone
        if (Mathf.Abs(horizontal) < stickDeadzone)
            horizontal = 0f;
        else
            horizontal = Mathf.Sign(horizontal) *
                         Mathf.InverseLerp(stickDeadzone, 1f, Mathf.Abs(horizontal));

        // Steering
        currentOffset += horizontal * rotationSpeed * Time.fixedDeltaTime;
        currentOffset = Mathf.Clamp(currentOffset, -maxAngleOffset, maxAngleOffset);

        // Player base angle
        float playerAngle = Mathf.Atan2(playerForward.y, playerForward.x) * Mathf.Rad2Deg - 90f;
        float finalAngle = playerAngle + currentOffset;

        // Rotate cart
        rb.MoveRotation(finalAngle);

        // Keep cart in front
        Vector2 offset = Quaternion.Euler(0, 0, finalAngle) * Vector2.up * distanceFromPlayer;
        rb.MovePosition((Vector2)player.position + offset);
    }
}
