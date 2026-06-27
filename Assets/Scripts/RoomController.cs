using System.Collections;
using UnityEngine;

public enum RoomType { Safe, Fight }

public class RoomController : MonoBehaviour
{
    public RoomType roomType;
    public BoxCollider2D roomBounds; // Коллайдер, определяющий границы комнаты

    [Header("Fight Room Settings")]
    public int enemiesToSpawn = 3; // Врагов в комнате
    public float spawnInterval = 0.5f;
    private int _aliveEnemies = 0;
    private bool _isRoomActive = false;

    [Header("Safe Room Settings")]
    public GameObject chairObject;    // Кресло (лечение)
    public GameObject levelUpTable;   // Стол (прокачка)

    public bool isCleared = false;

    private void Start()
    {
        roomBounds.isTrigger = true;
        if (roomType == RoomType.Safe && chairObject != null)
        {
            chairObject.SetActive(false);
            levelUpTable.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCleared)
        {
            ActivateRoom();
        }
    }

    private void ActivateRoom()
    {
        _isRoomActive = true;

        if (roomType == RoomType.Fight)
        {
            StartCoroutine(SpawnWave());
        }
        else if (roomType == RoomType.Safe)
        {
            if (chairObject != null) chairObject.SetActive(true);
            if (levelUpTable != null) levelUpTable.SetActive(true);
            Debug.Log("Вы в безопасной комнате. Можно подлечиться!");
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 spawnPos = GetRandomPointInBounds();
            // Передаем саму комнату (this.transform) как родителя
            PoolManager.Instance.SpawnEnemy(spawnPos, this.transform);
            _aliveEnemies++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void EnemyDied()
    {
        _aliveEnemies--;
        if (_aliveEnemies <= 0 && _isRoomActive)
        {
            ClearRoom();
        }
    }

    private void ClearRoom()
    {
        isCleared = true;
        _isRoomActive = false;
        Debug.Log("Комната очищена!");
    }

    private Vector2 GetRandomPointInBounds()
    {
        Bounds bounds = roomBounds.bounds;
        float x = Random.Range(bounds.min.x + 1f, bounds.max.x - 1f);
        float y = Random.Range(bounds.min.y + 1f, bounds.max.y - 1f);
        return new Vector2(x, y);
    }
}