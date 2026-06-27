using UnityEngine;

public class HealingChair : MonoBehaviour
{
    public float healAmount = 20f;
    public float healCooldown = 2f;
    private float _nextHealTime;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= _nextHealTime)
        {
            if (Input.GetKey(KeyCode.E)) // Зажмите E, чтобы лечиться
            {
                PlayerStats.Instance.Heal(healAmount);
                _nextHealTime = Time.time + healCooldown;
            }
        }
    }
}