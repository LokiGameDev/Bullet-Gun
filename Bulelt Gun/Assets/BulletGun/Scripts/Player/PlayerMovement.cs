using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform playerMesh;

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
        if(playerAnimation!=null) playerAnimation.SetMovement(0f);
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
        if(IsGrounded())
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimation.SetJumping();
            if(Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0.5f, 1f);
                Invoke(nameof(StopGamepadVibration), 0.2f);
            }
        }
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
            playerAnimation.SetCrouching(true);
            crouchMultiplier = 0.5f;
            sprintMultiplier = 1f;
            IsOnCrouch = true;
        }
        else
        {
            playerAnimation.SetCrouching(false);
            crouchMultiplier = 1f;
            IsOnCrouch = false;
        }
    }

    void Update()
    {
        playerAnimation.SetGrounded(IsGrounded());
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        
        playerRigidbody.linearVelocity = moveDirection.normalized * moveSpeed * sprintMultiplier * crouchMultiplier + new Vector3(0, playerRigidbody.linearVelocity.y, 0);
        RotatePlayerMesh(moveDirection);

        if(playerAnimation!=null) playerAnimation.SetMovement(moveDirection.magnitude/2*sprintMultiplier);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, originalHeight);
    }

    private void StopGamepadVibration()
    {
        if(Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    private void RotatePlayerMesh(Vector3 moveDirection)
    {
        Vector3 lookDir = new Vector3(
            moveDirection.x,
            0f,
            moveDirection.z
        );

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            playerMesh.transform.rotation = Quaternion.Slerp(
                playerMesh.transform.rotation,
                targetRotation,
                10 * Time.deltaTime
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * originalHeight);
    }
}
