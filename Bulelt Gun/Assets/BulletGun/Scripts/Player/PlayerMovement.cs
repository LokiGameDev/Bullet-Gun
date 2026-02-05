using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1f;
    [SerializeField] private float crouchMultiplier = 1f;
    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float originalHeight = 0.95f;

    private Vector2 movementInput;
    private Rigidbody playerRigidbody;
    private bool IsOnCrouch = false;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputReader.OnPlayerMovement += HandlePlayerMovement;
        inputReader.OnPlayerJump += HandlePlayerJump;
        inputReader.OnPlayerSprint += HandlePlayerSprint;
        inputReader.OnPlayerCrouch += HandlePlayerCrouch;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerMovement -= HandlePlayerMovement;
        inputReader.OnPlayerJump -= HandlePlayerJump;
        inputReader.OnPlayerSprint -= HandlePlayerSprint;
        inputReader.OnPlayerCrouch -= HandlePlayerCrouch;
    }

    private void HandlePlayerMovement(Vector2 movement)
    {
        movementInput = movement;
    }

    private void HandlePlayerJump()
    {
        if(IsGrounded()) playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void HandlePlayerSprint(bool isSprinting)
    {
        if(IsOnCrouch) return;
        sprintMultiplier = isSprinting ? 2f : 1f;
    }

    private void HandlePlayerCrouch(bool isCrouching)
    {
        if(isCrouching)
        {
            crouchMultiplier = 0.5f;
            sprintMultiplier = 1f;
            IsOnCrouch = true;
        }
        else
        {
            crouchMultiplier = 1f;
            IsOnCrouch = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        
        playerRigidbody.linearVelocity = moveDirection.normalized * moveSpeed * sprintMultiplier * crouchMultiplier + new Vector3(0, playerRigidbody.linearVelocity.y, 0);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, originalHeight);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * originalHeight);
    }
}
