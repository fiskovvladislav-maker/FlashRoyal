using UnityEngine;

public class LevelUpTable : MonoBehaviour
{
    private bool _isPlayerNear = false; // Флаг: находится ли игрок рядом

    // Как только игрок вошел в зону стола
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
        }
    }

    // Как только игрок вышел из зоны стола
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
        }
    }

    // Проверяем нажатие каждый кадр
    void Update()
    {
        // ИЗМЕНЕНО: теперь проверяем нажатие клавиши E
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            PlayerStats stats = PlayerStats.Instance;
            
            if (stats.currentSouls >= stats.soulsToNextLevel)
            {
                LevelUpManager.Instance.OpenLevelUpUI();
            }
            else
            {
                Debug.Log("Недостаточно душ для повышения уровня!");
            }
        }
    }
}