using UnityEngine;

public class SoulCollectible : MonoBehaviour
{
    [Header("Settings")]
    public float attractionRadius = 3f; 
    public float attractSpeed = 6f;     

    private Transform _playerTransform;
    private bool _isBeingCollected = false;

    private void Start()
    {
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;
    }

    public void Initialize(Vector2 spawnPos)
    {
        transform.position = spawnPos;
        _isBeingCollected = false;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        if (!_isBeingCollected)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= attractionRadius)
            {
                _isBeingCollected = true;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, attractSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectSoul();
        }
    }

    private void CollectSoul()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddSouls(1);
        }
        gameObject.SetActive(false);
    }
}