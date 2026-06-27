using UnityEngine;

public class SoulCollectible : MonoBehaviour
{
    [Header("Settings")]
    public float attractionRadius = 1000f; // Радиус, с которого душа летит к игроку
    public float attractSpeed = 20f;     // Скорость притягивания

    private Transform _playerTransform;
    private bool _isBeingCollected = false;

    private void Start()
    {
        // Находим игрока один раз при запуске игры
        _playerTransform = FindObjectOfType<PlayerController>().transform;
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

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if (!_isBeingCollected)
        {
            // Если игрок в радиусе притяжения, начинаем лететь к нему
            if (distanceToPlayer <= attractionRadius)
            {
                _isBeingCollected = true;
            }
        }
        else
        {
            // Плавно летим к игроку
            transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, attractSpeed * Time.deltaTime);

            // Если долетели до игрока (почти коснулись)
            if (distanceToPlayer < 2f)
            {
                CollectSoul();
            }
        }
    }

        private void CollectSoul()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddSouls(1); // Одна душа за врага
        }
        gameObject.SetActive(false);
    }
}