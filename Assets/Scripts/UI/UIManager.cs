using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    public Slider healthSlider;
    public TextMeshProUGUI soulsText;
    public TextMeshProUGUI levelText;

    private void Awake() { Instance = this; }

    private void OnEnable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged += UpdateHealthUI;
            PlayerStats.Instance.OnSoulsChanged += UpdateSoulsUI;
            PlayerStats.Instance.OnLevelChanged += UpdateLevelUI;
            
            // Принудительно обновить при старте
            UpdateHealthUI();
            UpdateSoulsUI();
            UpdateLevelUI();
        }
    }

    private void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHealthUI;
            PlayerStats.Instance.OnSoulsChanged -= UpdateSoulsUI;
            PlayerStats.Instance.OnLevelChanged -= UpdateLevelUI;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null && PlayerStats.Instance != null)
        {
            // МЫ ПОЛНОСТЬЮ УБРАЛИ СТРОКУ: healthSlider.maxValue = ...
            // Теперь слайдер всегда сохраняет 300 (или что вы поставили в Инспекторе).
            
            // Обновляем только ЗНАЧЕНИЕ заполнения, а не максимальный размер шкалы.
            healthSlider.value = PlayerStats.Instance.currentHealth;
            
            // Принудительно перерисовываем UI
            Canvas.ForceUpdateCanvases();
        }
    }

    private void UpdateSoulsUI()
    {
        if (soulsText != null && PlayerStats.Instance != null)
            soulsText.text = $"Души: {PlayerStats.Instance.currentSouls}";
    }

    private void UpdateLevelUI()
    {
        if (levelText != null && PlayerStats.Instance != null)
            levelText.text = $"Ур. {PlayerStats.Instance.currentLevel}";
    }
}