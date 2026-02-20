using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance==null)
            {
                Debug.LogError("Game Manager is null");
            }
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
    }

    public Transform cameraTransform;
    public InputReader inputReader;
    public GameObject player;

    public void Start()
    {
        player.SetActive(true);
    }

    public void BulletSpawned()
    {
        Time.timeScale = 0.15f;
    }

    public void BulletDespawned()
    {
        Time.timeScale = 1;
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
