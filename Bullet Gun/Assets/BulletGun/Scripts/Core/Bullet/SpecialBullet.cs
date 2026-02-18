using System;
using UnityEngine;

public class SpecialBullet : MonoBehaviour
{
    [Header("Refernces")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject finalBulletPrefab;

    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;

    public static event Action OnBulletDespawn;

    private bool canSpawn=false;
    void OnEnable()
    {
        inputReader.OnPlayerShoot += HandlePlayerShoot;
        Invoke("SetShootStatus", 0.2f);
    }

    void OnDisable()
    {
        inputReader.OnPlayerShoot -= HandlePlayerShoot;
    }

    private void HandlePlayerShoot()
    {
        if(canSpawn)
        {
            Shoot();
            canSpawn=false;
        }
    }

    public void Initialize(Vector3 direction)
    {
        transform.forward = direction;
    }

    private void Shoot()
    {
        Instantiate(finalBulletPrefab, transform.position, GameManager.Instance.cameraTransform.rotation);
        Invoke("DelayDead",0.1f);
    }

    void DelayDead()
    {
        OnBulletDespawn?.Invoke();
        GameManager.Instance.BulletDespawned();
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if(lifetime <= 0f)
        {
            OnBulletDespawn?.Invoke();
            GameManager.Instance.BulletDespawned();
            Destroy(gameObject);
        }
    }

    void SetShootStatus()
    {
        canSpawn = true;
    }
}
