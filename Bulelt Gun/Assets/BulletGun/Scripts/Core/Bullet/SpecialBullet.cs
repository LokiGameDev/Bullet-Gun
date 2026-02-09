using System;
using UnityEngine;

public class SpecialBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    void OnEnable()
    {
        
    }

    public void Initialize(Vector3 direction)
    {
        transform.forward = direction;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if(lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
