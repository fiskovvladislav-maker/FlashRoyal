using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        if (slider == null) slider = GetComponent<Slider>();
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(); // Обновляем при старте
        }
    }

    private void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        if (slider != null && PlayerStats.Instance != null)
        {
            // МЫ УБРАЛИ СТРОКУ: slider.maxValue = PlayerStats.Instance.maxHealth;
            // Теперь слайдер НЕ ПЕРЕЗАПИСЫВАЕТ ваши настройки в инспекторе (например, 300).
            
            // Мы просто обновляем текущее значение полоски.
            slider.value = PlayerStats.Instance.currentHealth;
            
            // Принудительно перерисовываем интерфейс
            Canvas.ForceUpdateCanvases();

            Debug.Log($"Слайдер обновлен! ХП: {slider.value} / {slider.maxValue}");
        }
    }
}