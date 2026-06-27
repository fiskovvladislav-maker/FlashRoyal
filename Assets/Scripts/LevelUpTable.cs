using UnityEngine;

public class LevelUpTable : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Нажмите Пробел, чтобы повысить уровень
            {
                PlayerStats.Instance.LevelUp();
            }
        }
    }
}