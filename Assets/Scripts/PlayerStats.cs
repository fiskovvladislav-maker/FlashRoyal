using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Leveling")]
    public int currentLevel = 1;
    public int currentSouls = 0;
    public int soulsToNextLevel = 10; // Сколько душ нужно для первого уровня

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Восстановление здоровья
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Восстановлено {amount} HP. Текущее HP: {currentHealth}");
    }

    // Добавление душ (вызывается из скрипта души)
    public void AddSouls(int amount)
    {
        currentSouls += amount;
        Debug.Log($"Собрано {amount} душ. Всего: {currentSouls}");
        
        // Проверяем, хватает ли на повышение уровня
        if (currentSouls >= soulsToNextLevel)
        {
            Debug.Log("Душ хватает! Подойдите к столу, чтобы повысить уровень!");
        }
    }

    // Повышение уровня (вызывается столом)
    public void LevelUp()
    {
        if (currentSouls < soulsToNextLevel) return;

        currentSouls -= soulsToNextLevel;
        currentLevel++;
        soulsToNextLevel = Mathf.RoundToInt(soulsToNextLevel * 1.5f); // Увеличиваем цену для следующего уровня
        Debug.Log($"Повышение уровня! Текущий уровень: {currentLevel}");
        
        // Здесь потом будет открытие UI с выбором 3 карт
    }
}