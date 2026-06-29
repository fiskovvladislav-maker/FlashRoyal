using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public TextMeshProUGUI soulsText;
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        // Если по какой-то причине Instance еще не создан, пробуем найти скрипт на сцене
        if (PlayerStats.Instance == null)
        {
            PlayerStats stats = FindFirstObjectByType<PlayerStats>();
            if (stats != null)
            {
                // Принудительно инициализируем синглтон, если он есть на сцене
                PlayerStats.Instance = stats; 
            }
            else
            {
                Debug.LogError("PlayerStats не найден ни как синглтон, ни на сцене!");
                return;
            }
        }

        PlayerStats.Instance.OnSoulsChanged += UpdateSoulsUI;
        PlayerStats.Instance.OnLevelChanged += UpdateLevelUI;
        
        UpdateSoulsUI();
        UpdateLevelUI();
    }

    private void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnSoulsChanged -= UpdateSoulsUI;
            PlayerStats.Instance.OnLevelChanged -= UpdateLevelUI;
        }
    }

    private void UpdateSoulsUI()
    {
        if (soulsText != null && PlayerStats.Instance != null)
        {
            soulsText.text = $"Души: {PlayerStats.Instance.currentSouls}";
        }
    }

    private void UpdateLevelUI()
    {
        if (levelText != null && PlayerStats.Instance != null)
        {
            levelText.text = $"Ур. {PlayerStats.Instance.currentLevel}";
        }
    }
}