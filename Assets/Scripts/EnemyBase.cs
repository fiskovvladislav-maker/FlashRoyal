using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data; // Ссылка на файл
    private float _currentHp;
    private float _moveSpeed; // Внутренняя переменная для скорости
    private Transform _playerTransform;

    public void Initialize()
    {
        _currentHp = data.hp;
        _moveSpeed = data.moveSpeed;
        gameObject.SetActive(true);
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
        if (currentRoom != null) currentRoom.EnemyDied();
        PoolManager.Instance.ReturnEnemy(this);
    }
}