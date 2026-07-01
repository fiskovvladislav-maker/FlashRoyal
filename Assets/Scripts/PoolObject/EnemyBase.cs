using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHp = 10f; 
    public float moveSpeed = 2f;
    private float _currentHp;
    private Transform _playerTransform;

    public void Initialize()
    {
        _currentHp = maxHp;
        gameObject.SetActive(true);
        if (PlayerStats.Instance != null) _playerTransform = PlayerStats.Instance.transform;
    }

    void Update()
    {
        if (_playerTransform == null) return;
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0) Die();
    }

    private void Die()
    {
        RoomController currentRoom = GetComponentInParent<RoomController>();
        PoolManager.Instance.SpawnSoul(transform.position);
        currentRoom?.EnemyDied();
        PoolManager.Instance.ReturnEnemy(this);
    }
}