using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f; 
    
    private Vector2 _direction; 
    private float _damage; // Урон теперь переменная, а не константа

    public void Initialize(Vector2 direction)
    {
        _direction = direction;
        _damage = PlayerStats.Instance.damage; // Берем текущий урон из статов!
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }
}