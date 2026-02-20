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

    private bool canShoot;
    private InputReader inputReader;
    private Player player;

    private void Start()
    {
        canShoot = true;
        player = GetComponent<Player>();
        player.SetCurrentActionObject(true);
        player.mainCamera.currentTarget = player.transform;
        player.mainCamera.currentCameraOffset = playerCameraNormalOffset;
    }

    private void OnEnable()
    {
        inputReader = GameManager.Instance.inputReader;
        inputReader.OnPlayerShoot += HandlePlayerAttack;
        inputReader.OnPlayerAim += HandlePlayerAim;
        SpecialBullet.OnBulletDespawn += HandleBulletDespawn;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerShoot -= HandlePlayerAttack;
        inputReader.OnPlayerAim -= HandlePlayerAim;
        SpecialBullet.OnBulletDespawn -= HandleBulletDespawn;
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
    }

    private void HandlePlayerAim(bool status)
    {
        if(status)
        {
            if(player.isPlayerOnAction)
            {
                player.mainCamera.currentCameraOffset = playerCameraAimOffset;
                player.playerAnimation.SetAiming(true);
                player.playerAnimation.PlayerMeshLookForward(transform.rotation);
            }
            else
            {
                player.mainCamera.currentCameraOffset = bulletCameraAimOffset;
                GameManager.Instance.SetTimeScale(0.05f);
            }
        }
        else
        {
            if(player.isPlayerOnAction)
            {
                player.mainCamera.currentCameraOffset = playerCameraNormalOffset;
            }
            else
            {
                player.mainCamera.currentCameraOffset = bulletCameraNormalOffset;
                GameManager.Instance.SetTimeScale(0.15f);
            }
            player.playerAnimation.SetAiming(false);
        }
        player.SetPlayerAimStatus(status);
    }

    private void PlayerShoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpecialBullet>().Initialize(firePoint.forward);
        reloadImageFill.fillAmount = 0;
        BulletSpawned(bullet);
    }

    private void BulletShoot()
    {
        
    }

    private void BulletSpawned(GameObject bullet)
    {
        player.SetCurrentActionObject(false);
        player.mainCamera.currentTarget = bullet.transform;
        player.mainCamera.currentCameraOffset = bulletCameraNormalOffset;
        GameManager.Instance.BulletSpawned();
    }

    private void HandleBulletDespawn()
    {
        player.SetCurrentActionObject(true);
        player.mainCamera.currentTarget = player.transform;
        player.mainCamera.currentCameraOffset = playerCameraNormalOffset;
        GameManager.Instance.BulletDespawned();
    }

    private IEnumerator PlayerShootDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        canShoot = true;
        reloadImageFill.fillAmount = 1;
    }
}
