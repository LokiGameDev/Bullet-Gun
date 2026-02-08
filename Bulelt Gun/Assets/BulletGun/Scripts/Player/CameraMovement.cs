using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Vector3 normalCameraOffset = new Vector3(0f, 0.5f, -2f);
    [SerializeField] private Vector3 aimCameraOffset = new Vector3(0f, 0.25f, -1.5f);

    private Vector2 lookInput;

    private Vector3 currentCameraOffset;


    float yaw;

    private void OnEnable()
    {
        inputReader.OnPlayerLook += HandlePlayerLook;
        inputReader.OnPlayerAttack += HandlePlayerAim;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerLook -= HandlePlayerLook;
        inputReader.OnPlayerAttack -= HandlePlayerAim;
    }

    private void HandlePlayerLook()
    {
        lookInput = inputReader.lookInput;
    }

    private void HandlePlayerAim(bool isAiming)
    {
        SetAiming(isAiming);
    }

    private void Start()
    {
        currentCameraOffset = normalCameraOffset;
    }

    public void SetAiming(bool isAiming)
    {
        if(isAiming)
        {
            currentCameraOffset = aimCameraOffset;
        }
        else
        {
            currentCameraOffset = normalCameraOffset;
        }
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        transform.position = cameraTarget.position;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentCameraOffset, Time.deltaTime * 10f);
    }
}
