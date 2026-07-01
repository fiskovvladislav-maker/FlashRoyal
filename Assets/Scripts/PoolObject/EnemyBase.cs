using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data; 

    private float _currentHp;
    private float _moveSpeed; 
    private Transform _playerTransform;

    public void Initialize()
    {
        if (data == null)
        {
            Debug.LogError("На враге не назначен EnemyDataSO! Проверьте префаб.");
            return;
        }

        _currentHp = data.hp;
        _moveSpeed = data.moveSpeed;

        gameObject.SetActive(true);
        if (PlayerStats.Instance != null) 
            _playerTransform = PlayerStats.Instance.transform;
    }

    void Update()
    {
        if (_playerTransform == null) return;
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _moveSpeed * Time.deltaTime);
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