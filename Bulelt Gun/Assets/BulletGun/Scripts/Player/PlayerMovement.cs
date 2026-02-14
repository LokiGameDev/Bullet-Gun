using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject playerMesh;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float meshRotationSpeed = 100f;
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
        if(cameraMovement.IsAiming) {return;}

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
        if(cameraMovement.IsAiming) {return;}
        
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
        if(cameraMovement.IsAiming) {return;}

        Vector3 moveDirection = ( (transform.forward * movementInput.y) + (transform.right * movementInput.x))* moveSpeed * sprintMultiplier * crouchMultiplier;

        playerRigidbody.linearVelocity = new Vector3(moveDirection.x, playerRigidbody.linearVelocity.y, moveDirection.z);

        if (movementInput != Vector2.zero)
        {
            RotatePlayerMesh(moveDirection);
        }

        Vector3 flatVelocity = playerRigidbody.linearVelocity;
        flatVelocity.y = 0f;

        if (flatVelocity.sqrMagnitude > 0.01f)
        {
            float dot = Vector3.Dot(transform.forward, flatVelocity.normalized);

            // Only rotate if not strongly moving backwards
            if (dot > -0.5f)   // adjust threshold if needed
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatVelocity);

                Quaternion newRotation = Quaternion.Slerp(
                    playerRigidbody.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

                playerRigidbody.MoveRotation(newRotation);
            }
        }

        if(playerAnimation!=null) playerAnimation.SetMovement(moveDirection.magnitude/(moveSpeed*2)*sprintMultiplier);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, originalHeight);
    }

    public void MeshLookForward()
    {
        playerMesh.transform.rotation = transform.rotation;
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
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        playerMesh.transform.rotation = Quaternion.Slerp(
            playerMesh.transform.rotation,
            targetRotation,
            meshRotationSpeed * Time.fixedDeltaTime
        );
    }

    public void SetAiming(bool isAiming)
    {
        playerAnimation.SetAiming(isAiming);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * originalHeight);
    }
}
