using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Player player;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 60;

    public Transform currentTarget;
    public Vector3 currentCameraOffset;
    private Vector2 lookInput;

    float yaw;
    float xRotation = 0;
    float yRotation = 0;

    private void OnEnable()
    {
        inputReader.OnPlayerLook += HandlePlayerLook;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerLook -= HandlePlayerLook;
    }

    private void HandlePlayerLook()
    {
        lookInput = inputReader.lookInput;
    }

    private void LateUpdate()
    {
        if(currentTarget==null)
        {
            currentTarget = player.gameObject.transform;   
        }
        
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        
        if(lookInput.magnitude < 0.1f)
        {
            yaw = Mathf.LerpAngle(yaw, 0, Time.deltaTime * 10f);
        }

        if(player.isPlayerOnAction)
        {
            transform.rotation = currentTarget.rotation * Quaternion.Euler(0, yaw, 0f);  
        }
        else
        {
            xRotation -= lookInput.y;
            yRotation += lookInput.x;
            xRotation = Mathf.Clamp(xRotation, -30f, 30f);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }

        transform.position = currentTarget.position;
        // cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentCameraOffset, Time.deltaTime * 10f);
        cameraTransform.localPosition = currentCameraOffset;
    }


}
