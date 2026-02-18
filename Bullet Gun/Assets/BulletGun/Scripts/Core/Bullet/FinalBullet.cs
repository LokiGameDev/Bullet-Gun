using UnityEngine;

public class FinalBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifetime = 3;
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
