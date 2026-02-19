using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Image reloadImageFill;

    [Header("Camera Offsets")]
    [SerializeField] private Vector3 playerCameraNormalOffset;
    [SerializeField] private Vector3 playerCameraAimOffset;
    [SerializeField] private Vector3 bulletCameraNormalOffset;
    [SerializeField] private Vector3 bulletCameraAimOffset;
    
    public static event Action<Transform> OnBulletSpawned;

    private bool canShoot;
    private InputReader inputReader;
    private Player player;

    private void Start()
    {
        canShoot = true;
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        inputReader = GameManager.Instance.inputReader;
        inputReader.OnPlayerShoot += HandlePlayerAttack;
        inputReader.OnPlayerAim += HandlePlayerAim;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerShoot -= HandlePlayerAttack;
        inputReader.OnPlayerAim -= HandlePlayerAim;
    }

    private void HandlePlayerAttack()
    {
        if(!canShoot) return;

        if(player.isPlayerOnAction)
        {
            PlayerShoot();
            canShoot = false;
            StartCoroutine(PlayerShootDelay(1));
        }
        else
        {
            BulletShoot();
            canShoot = false;
            StartCoroutine(PlayerShootDelay(3));
        }
    }

    private void HandlePlayerAim(bool status)
    {
        if(status)
        {
            if(player.isPlayerOnAction) player.mainCamera.currentCameraOffset = playerCameraAimOffset;
            else player.mainCamera.currentCameraOffset = bulletCameraAimOffset;
        }
        else
        {
            if(player.isPlayerOnAction) player.mainCamera.currentCameraOffset = playerCameraNormalOffset;
            else player.mainCamera.currentCameraOffset = bulletCameraNormalOffset;
        }
        player.SetPlayerAimStatus(status);
    }

    private void PlayerShoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpecialBullet>().Initialize(firePoint.forward);
        OnBulletSpawned?.Invoke(bullet.transform);
        GameManager.Instance.BulletSpawned();
    }

    private void BulletShoot()
    {
        
    }

    private IEnumerator PlayerShootDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        canShoot = true;
    }
}
