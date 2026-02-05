using UnityEngine;
using System;
using UnityEngine.InputSystem;
using static PlayerInputActions;


[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    #region Actions
    public event Action<Vector2> OnPlayerMovement;
    public event Action<bool> OnPlayerSprint;
    public event Action<bool> OnPlayerCrouch;
    public event Action OnPlayerLook;
    public event Action OnPlayerJump;
    // public event Action OnPlayerAttack;
    // public event Action OnPlayerInteract;
    #endregion

    #region Input Values
    public Vector2 lookInput;
    #endregion

    private PlayerInputActions playerInputActions;
    void OnEnable()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.SetCallbacks(this);
        }
        playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Nothing here
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        OnPlayerCrouch?.Invoke(context.performed);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Nothing here
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnPlayerJump?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        OnPlayerLook?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnPlayerMovement?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        OnPlayerSprint?.Invoke(context.performed);
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        // Nothing here
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        // Nothing here
    }

}
