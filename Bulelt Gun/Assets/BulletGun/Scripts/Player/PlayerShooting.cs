using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Image reloadImageFill;

    [Header("Settings")]
    [SerializeField] private float shootCooldown = 1f;

    private bool canShoot;
    private float shootTimer;

    private void OnEnable()
    {
        canShoot = true;
        inputReader.OnPlayerShoot += HandlePlayerAttack;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerShoot -= HandlePlayerAttack;
    }

    private void HandlePlayerAttack()
    {
        if(canShoot)
        {
            Shoot();
            canShoot = false;
            shootTimer = shootCooldown;
            StartCoroutine(ResetShootCooldown());
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpecialBullet>().Initialize(firePoint.forward);
    }

    IEnumerator ResetShootCooldown()
    {
        reloadImageFill.fillAmount = 0f;
        while(shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
            reloadImageFill.fillAmount = 1f - (shootTimer / shootCooldown);
            yield return null;
        }
        canShoot = true;
    }
}
