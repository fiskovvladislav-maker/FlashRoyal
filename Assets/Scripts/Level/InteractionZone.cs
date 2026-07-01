using UnityEngine;

public enum InteractionType { Chair, Table, ExitDoor }

public class InteractionZone : MonoBehaviour
{
    public InteractionType type;
    private bool _isPlayerNear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _isPlayerNear = false;
    }

    private void Update()
    {
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            switch (type)
            {
                case InteractionType.Chair:
                    if (PlayerStats.Instance != null)
                        PlayerStats.Instance.Heal(20f);
                    break;
                case InteractionType.Table:
                    if (PlayerStats.Instance != null && PlayerStats.Instance.currentSouls >= PlayerStats.Instance.soulsToNextLevel)
                        LevelUpManager.Instance.OpenLevelUpUI();
                    break;
                case InteractionType.ExitDoor:
                    Debug.Log("Выход из игры...");
                    Application.Quit(); // Закрывает приложение
                    break;
            }
        }
    }
}