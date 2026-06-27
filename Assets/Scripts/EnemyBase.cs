using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHp = 10f; 
    public float moveSpeed = 2f; // НОВОЕ: Скорость передвижения врага
    
    private float _currentHp;
    private Transform _playerTransform; // Чтобы запомнить, куда бежать

    public void Initialize()
    {
        _currentHp = maxHp;
        gameObject.SetActive(true);
        
        // Запоминаем позицию игрока при появлении врага
        if (PlayerStats.Instance != null)
        {
            _playerTransform = PlayerStats.Instance.transform;
        }
    }

    void Update() // НОВЫЙ МЕТОД: Заставляет врага двигаться каждый кадр
    {
        if (_playerTransform == null) return;

        // Плавно двигаемся в сторону игрока
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        RoomController currentRoom = GetComponentInParent<RoomController>();
        PoolManager.Instance.SpawnSoul(transform.position);
        if (currentRoom != null) currentRoom.EnemyDied();
        PoolManager.Instance.ReturnEnemy(this);
    }
}