using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Image reloadImageFill;

    public static event Action<Transform> OnBulletSpawned;

    private bool canShoot;

    private void OnEnable()
    {
        canShoot = true;
        inputReader.OnPlayerShoot += HandlePlayerAttack;
        SpecialBullet.OnBulletDespawn += HandleBulletDespawn;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerShoot -= HandlePlayerAttack;
        SpecialBullet.OnBulletDespawn -= HandleBulletDespawn;
    }

    private void HandlePlayerAttack()
    {
        if(canShoot)
        {
            Shoot();
            canShoot = false;
            reloadImageFill.fillAmount = 0;
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpecialBullet>().Initialize(firePoint.forward);
        OnBulletSpawned?.Invoke(bullet.transform);
        GameManager.Instance.BulletSpawned();
    }

    private void HandleBulletDespawn()
    {
        canShoot = true;
        reloadImageFill.fillAmount = 1;
    }
}
