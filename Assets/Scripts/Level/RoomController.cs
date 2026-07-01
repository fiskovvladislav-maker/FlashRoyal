using System.Collections;
using UnityEngine;

public enum RoomType { Safe, Fight }

public class RoomController : MonoBehaviour
{
    public RoomType roomType;
    public BoxCollider2D roomBounds;

    [Header("Fight Room")]
    public int enemiesToSpawn = 3;
    public float spawnInterval = 0.5f;
    
    private int _aliveEnemies = 0;
    private bool _hasSpawned = false;

    [Header("Safe Room")]
    public GameObject chairObject;
    public GameObject levelUpTable;

    private void Start()
    {
        roomBounds.isTrigger = true;
        if (roomType == RoomType.Safe)
        {
            chairObject?.SetActive(false);
            levelUpTable?.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasSpawned) ActivateRoom();
    }

    private void ActivateRoom()
    {
        _hasSpawned = true;

        switch (roomType)
        {
            case RoomType.Fight:
                StartCoroutine(SpawnWave());
                break;
            case RoomType.Safe:
                chairObject?.SetActive(true);
                levelUpTable?.SetActive(true);
                break;
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            PoolManager.Instance.SpawnEnemy(GetRandomPointInBounds(), transform);
            _aliveEnemies++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void EnemyDied()
    {
        _aliveEnemies--;
        if (_aliveEnemies <= 0) Debug.Log("Комната зачищена!");
    }

    private Vector2 GetRandomPointInBounds()
    {
        Bounds bounds = roomBounds.bounds;
        return new Vector2(
            Random.Range(bounds.min.x + 1f, bounds.max.x - 1f),
            Random.Range(bounds.min.y + 1f, bounds.max.y - 1f)
        );
    }
}