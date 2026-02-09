using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private void Start()
    {
        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }
    }

    public void SetMovement(float speed)
    {
        playerAnimator.SetFloat("Speed", speed);
    }

    public void SetJumping()
    {
        playerAnimator.SetTrigger("Jump");
    }

    public void SetGrounded(bool isGrounded)
    {
        playerAnimator.SetBool("Grounded", isGrounded);
    }

    public void SetCrouching(bool isCrouching)
    {
        playerAnimator.SetBool("Crouching", isCrouching);
    }

    public void SetAiming(bool isAiming)
    {
        playerAnimator.SetBool("Aiming", isAiming);
    }
}
