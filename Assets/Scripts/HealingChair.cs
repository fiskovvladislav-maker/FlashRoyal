using UnityEngine;

public class HealingChair : MonoBehaviour
{
    public float healAmount = 20f;
    public float healCooldown = 2f; // Кулдаун между лечениями

    private float _nextHealTime;
    private bool _isPlayerNear = false; // Флаг: рядом ли игрок

    // Игрок зашел в зону кресла
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = true;
        }
    }

    // Игрок вышел из зоны кресла
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNear = false;
        }
    }

    void Update()
    {
        // Если игрок рядом, клавиша E нажата, и кулдаун прошел
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E) && Time.time >= _nextHealTime)
        {
            PlayerStats.Instance.Heal(healAmount);
            _nextHealTime = Time.time + healCooldown; // Сбрасываем кулдаун
            Debug.Log($"Здоровье востановленно");
        }
    }
}