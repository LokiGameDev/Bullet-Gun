using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform cameraTargetPlayer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 60;
    [SerializeField] private Vector3 normalCameraOffset = new Vector3(0f, 0.5f, -2f);
    [SerializeField] private Vector3 aimCameraOffset = new Vector3(0f, 0.25f, -1.5f);
    [SerializeField] private Vector3 bulletCameraOffset = new Vector3(0, 0.2f, -1f);
    [SerializeField] private Vector3 bulletAimCameraOffset = new Vector3(0f, 0.15f, -0.5f);

    public bool IsAiming { get; private set; }

    private Vector2 lookInput;

    private Vector3 currentCameraOffset;

    private Transform currentTarget;
    private Vector3 currentNormalOffset;


    float yaw;
    float xRotation = 0;
    float yRotation = 0;

    private void OnEnable()
    {
        inputReader.OnPlayerLook += HandlePlayerLook;
        inputReader.OnPlayerAim += HandlePlayerAim;
        PlayerShooting.OnBulletSpawned += HandleBulletSpawn;
        SpecialBullet.OnBulletDespawn += HandleBulletDespawn;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerLook -= HandlePlayerLook;
        inputReader.OnPlayerAim -= HandlePlayerAim;
        PlayerShooting.OnBulletSpawned -= HandleBulletSpawn;
        SpecialBullet.OnBulletDespawn -= HandleBulletDespawn;
    }

    private void HandlePlayerLook()
    {
        lookInput = inputReader.lookInput;
    }

    private void HandlePlayerAim(bool isAiming)
    {
        SetAiming(isAiming);
    }

    private void HandleBulletSpawn(Transform bullet)
    {
        currentTarget = bullet;
        mouseSensitivity = 1;
        yRotation = 0;
        xRotation = 0;
    }

    private void HandleBulletDespawn()
    {
        currentTarget = cameraTargetPlayer;
        transform.position = currentTarget.position;
        currentCameraOffset = normalCameraOffset;
        mouseSensitivity = 60;
    }

    private void Start()
    {
        currentCameraOffset = normalCameraOffset;
        currentTarget = cameraTargetPlayer;
    }

    public void SetAiming(bool isAiming)
    {
        if(isAiming)
        {
            IsAiming = true;
            playerMovement.MeshLookForward();
            if(currentTarget==cameraTargetPlayer)
            {
                currentCameraOffset = aimCameraOffset;
                Time.timeScale = 0.5f;
            }
            else
            {
                currentCameraOffset = bulletAimCameraOffset;
                Time.timeScale = 0.1f;
            }
            
        }
        else
        {
            IsAiming = false;
            if(currentTarget==cameraTargetPlayer)
            {
                currentCameraOffset = normalCameraOffset;
                Time.timeScale = 1f;
            }
            else
            {
                currentCameraOffset = bulletCameraOffset;
                Time.timeScale = 0.25f;
            }
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

        if(currentTarget==cameraTargetPlayer)
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
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, currentCameraOffset, Time.deltaTime * 10f);
    }


}
