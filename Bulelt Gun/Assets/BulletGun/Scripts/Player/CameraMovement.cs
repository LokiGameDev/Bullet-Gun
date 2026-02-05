using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform playerBody;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    private Vector2 lookInput;


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
        transform.position = playerBody.position;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        transform.rotation = Quaternion.Euler(0f, playerBody.eulerAngles.y, 0f);
    }
}
