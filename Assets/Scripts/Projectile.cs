using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f; // Скорость пули
    public int damage = 10;  // Количество урона, которое наносит пуля
    
    private Vector2 _direction; // Направление полета

    // Метод, чтобы задать направление пуле
    public void Initialize(Vector2 direction)
    {
        _direction = direction;
        gameObject.SetActive(true); // Включаем объект
    }

    void Update()
    {
        // Двигаем пулю по направлению каждый кадр
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    // При столкновении с триггером
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если задели врага (по тегу)
        if (other.CompareTag("Enemy"))
        {
            // 1. Наносим урон врагу (вызываем его скрипт)
            other.GetComponent<EnemyBase>().TakeDamage(damage);
            
            // 2. Убираем пулю
            gameObject.SetActive(false);
        }
    }
}