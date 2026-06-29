using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Vector2 _lastMoveDir = Vector2.right; // Запоминаем последнее направление

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashCooldown = 1.5f;
    private float _nextDashTime = 0f; // Изначально 0, чтобы можно было делать рывок сразу

    [Header("Combat")]
    public Transform firePoint; 
    public float aimRadius = 7f;  
    public LayerMask enemyLayer;  

    private float _nextFireTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Ввод движения
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // Если мы двигаемся, запоминаем это направление для рывка
        if (_moveInput != Vector2.zero)
        {
            _lastMoveDir = _moveInput.normalized;
        }

        // 2. Рывок (Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= _nextDashTime)
        {
            Debug.Log("Кнопка Shift нажата! Пытаюсь сделать рывок..."); // Проверка
            PerformDash();
            _nextDashTime = Time.time + dashCooldown;
        }

        // 3. Автоматическая стрельба
        if (Time.time >= _nextFireTime)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, aimRadius, enemyLayer);
            
            Transform nearestTarget = null;
            float minDist = float.MaxValue;

            foreach (var enemy in enemiesInRange)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestTarget = enemy.transform;
                }
            }

            if (nearestTarget != null)
            {
                Shoot(nearestTarget);
                _nextFireTime = Time.time + PlayerStats.Instance.fireRate; 
            }
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput.normalized * speed * Time.fixedDeltaTime);
    }

        private void PerformDash()
    {
        Vector2 direction = _lastMoveDir != Vector2.zero ? _lastMoveDir : Vector2.right;
        
        // ВАЖНОЕ ИЗМЕНЕНИЕ: Мы не используем MovePosition, мы обновляем позицию напрямую.
        // Это заставит игрока телепортироваться на расстояние рывка мгновенно.
        _rb.position = _rb.position + direction * dashDistance;
        
        Debug.Log($"Рывок выполнен! Новое положение: {_rb.position}");
    }

    private void Shoot(Transform target)
    {
        Projectile bullet = PoolManager.Instance.GetBullet();
        if (bullet == null) return;

        bullet.transform.position = firePoint.position;
        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.Initialize(direction);
    }

        // НОВЫЙ МЕТОД: Автоматически вызывается при физическом столкновении
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Наносим урон игроку (например, 10 единиц за касание)
            PlayerStats.Instance.TakeDamage(10f);
        }
    }
}