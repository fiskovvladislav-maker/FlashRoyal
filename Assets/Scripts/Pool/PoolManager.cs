using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int enemyPoolSize = 20;
    private ObjectPool<EnemyBase> _enemyPool;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public int bulletPoolSize = 30;
    private ObjectPool<Projectile> _bulletPool;

    [Header("Soul Settings")]
    public GameObject soulPrefab;
    public int soulPoolSize = 30;
    private ObjectPool<SoulCollectible> _soulPool;

    void Awake() { Instance = this; }

    void Start()
    {
        if (enemyPrefab != null) _enemyPool = new ObjectPool<EnemyBase>(enemyPrefab.GetComponent<EnemyBase>(), enemyPoolSize, transform);
        if (bulletPrefab != null) _bulletPool = new ObjectPool<Projectile>(bulletPrefab.GetComponent<Projectile>(), bulletPoolSize, transform);
        if (soulPrefab != null) _soulPool = new ObjectPool<SoulCollectible>(soulPrefab.GetComponent<SoulCollectible>(), soulPoolSize, transform);
    }

    public void SpawnEnemy(Vector2 position, Transform parent)
    {
        if (_enemyPool == null) return;
        EnemyBase enemy = _enemyPool.Get();
        enemy.transform.position = position;
        enemy.transform.SetParent(parent); 
        enemy.Initialize();
    }
    public void ReturnEnemy(EnemyBase enemy) => _enemyPool?.ReturnToPool(enemy);

    public Projectile GetBullet()
    {
        if (_bulletPool == null) return null;
        return _bulletPool.Get();
    }
    public void ReturnBullet(Projectile bullet) => _bulletPool?.ReturnToPool(bullet);

    public void SpawnSoul(Vector2 position)
    {
        if (_soulPool == null) return;
        SoulCollectible soul = _soulPool.Get();
        soul.transform.position = position;
        soul.Initialize(position);
    }
    public void ReturnSoul(SoulCollectible soul) => _soulPool?.ReturnToPool(soul);
}