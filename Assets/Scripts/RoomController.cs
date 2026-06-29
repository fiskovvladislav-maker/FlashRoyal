using System.Collections;
using UnityEngine;

public enum RoomType { Safe, Fight }

public class RoomController : MonoBehaviour
{
    public RoomType roomType;
    public BoxCollider2D roomBounds;

    [Header("Fight Room Settings")]
    public int enemiesToSpawn = 3;
    public float spawnInterval = 0.5f;
    
    private int _aliveEnemies = 0;
    private bool _hasSpawned = false; // Единственный флаг: спавнили ли мы уже врагов?

    [Header("Safe Room Settings")]
    public GameObject chairObject;
    public GameObject levelUpTable;

    private void Start()
    {
        roomBounds.isTrigger = true;
        if (roomType == RoomType.Safe)
        {
            if (chairObject != null) chairObject.SetActive(false);
            if (levelUpTable != null) levelUpTable.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если вошел игрок, И мы еще не спавнили врагов -> активируем комнату
        if (other.CompareTag("Player") && !_hasSpawned)
        {
            ActivateRoom();
        }
    }

    private void ActivateRoom()
    {
        _hasSpawned = true; // Отмечаем, что комната активирована

        if (roomType == RoomType.Fight)
        {
            StartCoroutine(SpawnWave());
        }
        else if (roomType == RoomType.Safe)
        {
            if (chairObject != null) chairObject.SetActive(true);
            if (levelUpTable != null) levelUpTable.SetActive(true);
            Debug.Log("Вы в безопасной комнате.");
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 spawnPos = GetRandomPointInBounds();
            PoolManager.Instance.SpawnEnemy(spawnPos, this.transform);
            _aliveEnemies++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Этот метод вызывается скриптом EnemyBase, когда враг умирает
    public void EnemyDied()
    {
        _aliveEnemies--;
        if (_aliveEnemies <= 0)
        {
            Debug.Log("Комната зачищена!");
            // Здесь можно разблокировать двери, если они будут нужны
        }
    }

    private Vector2 GetRandomPointInBounds()
    {
        Bounds bounds = roomBounds.bounds;
        float x = Random.Range(bounds.min.x + 1f, bounds.max.x - 1f);
        float y = Random.Range(bounds.min.y + 1f, bounds.max.y - 1f);
        return new Vector2(x, y);
    }
}