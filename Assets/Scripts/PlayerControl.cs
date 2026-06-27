using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    [Header("Combat")]
    public Transform firePoint; // Точка, откуда вылетает пуля
    public float fireRate = 0.5f; // Скорострельность (раз в 0.5 секунды)
    public float aimRadius = 7f;  // Радиус, в котором ищем врагов
    public LayerMask enemyLayer;  // Слой, на котором находятся враги

    private float _nextFireTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Ввод движения
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 2. Автоматическая стрельба
        if (Time.time >= _nextFireTime)
        {
            // Ищем всех врагов в радиусе aimRadius
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, aimRadius, enemyLayer);
            
            Transform nearestTarget = null;
            float minDist = float.MaxValue;

            // Перебираем найденных врагов и ищем ближайшего
            foreach (var enemy in enemiesInRange)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestTarget = enemy.transform;
                }
            }

            // Если враг найден, стреляем в него
            if (nearestTarget != null)
            {
                Shoot(nearestTarget);
                _nextFireTime = Time.time + fireRate; // Сбрасываем таймер
            }
        }
    }

    void FixedUpdate()
    {
        // Двигаем физику
        _rb.MovePosition(_rb.position + _moveInput.normalized * speed * Time.fixedDeltaTime);
    }

    private void Shoot(Transform target)
    {
        // Берем пулю из пула
        Projectile bullet = PoolManager.Instance.GetBullet();
        if (bullet == null) return;

        // Ставим пулю на позицию firePoint
        bullet.transform.position = firePoint.position;
        
        // Считаем направление от игрока в сторону врага
        Vector2 direction = (target.position - firePoint.position).normalized;
        
        // Инициализируем пулю (включаем её и задаем направление)
        bullet.Initialize(direction);
    }
}