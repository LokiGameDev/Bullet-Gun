using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CameraMovement mainCamera;
    public PlayerAnimation playerAnimation;
    public bool canMove { get; private set; }
    public bool isAiming { get; private set; }
    [field:SerializeField] public bool isMoving { get; private set; }
    public bool isPlayerOnAction { get; private set; }

    public Quaternion lookingDirection;
    public bool isLookChanged = false;

    void Awake()
    {
        canMove = true;
        isAiming = false;
        isPlayerOnAction = true;
    }

    public void SetPlayerMoveStatus(bool status)
    {
        canMove = status;
    }

    public void SetPlayerAimStatus(bool status)
    {
        isAiming = status;
    }

    public void SetCurrentActionObject(bool isPlayer)
    {
        isPlayerOnAction = isPlayer;
    }

    public void SetMovementStatus(bool status)
    {
        isMoving = status;
    }

    public void SetMainCameraTarget(Transform target)
    {
        mainCamera.currentTarget = target;
    }

    public void SetMainCameraOffset(Vector3 offset)
    {
        mainCamera.currentCameraOffset = offset;
    }

    public void SetMainCameraRotation(Quaternion rotation)
    {
        mainCamera.SetCameraRotation(rotation);
    }

    public Quaternion GetLookingDirection()
    {
        return lookingDirection;
    }
}