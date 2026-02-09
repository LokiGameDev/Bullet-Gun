using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Vector3 normalCameraOffset = new Vector3(0f, 0.5f, -2f);
    [SerializeField] private Vector3 aimCameraOffset = new Vector3(0f, 0.25f, -1.5f);

    public bool IsAiming { get; private set; }

    private Vector2 lookInput;

    private Vector3 currentCameraOffset;


    float yaw;

    private void OnEnable()
    {
        inputReader.OnPlayerLook += HandlePlayerLook;
        inputReader.OnPlayerAim += HandlePlayerAim;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerLook -= HandlePlayerLook;
        inputReader.OnPlayerAim -= HandlePlayerAim;
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
            IsAiming = true;
            currentCameraOffset = aimCameraOffset;
        }
        else
        {
            IsAiming = false;
            currentCameraOffset = normalCameraOffset;
        }
        playerMovement.SetAiming(isAiming);
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        if(lookInput.magnitude < 0.1f)
        {
            yaw = Mathf.LerpAngle(yaw, 0, Time.deltaTime * 10f);
        }
        transform.rotation = cameraTarget.rotation * Quaternion.Euler(0f, yaw, 0f);

        transform.position = cameraTarget.position;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentCameraOffset, Time.deltaTime * 10f);
    }
}
