using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    // События для обновления UI
    public event Action OnHealthChanged;
    public event Action OnSoulsChanged; // НОВОЕ: для счетчика душ
    public event Action OnLevelChanged; // НОВОЕ: для счетчика уровня

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Combat Stats")]
    public float damage = 10f;
    public float fireRate = 0.5f;

    [Header("Leveling")]
    public int currentLevel = 1;
    public int currentSouls = 0;
    public int soulsToNextLevel = 10; 

    void Awake() { Instance = this; }

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
        OnSoulsChanged?.Invoke();   // Обновляем души при старте
        OnLevelChanged?.Invoke();   // Обновляем уровень при старте
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke();
        if (currentHealth <= 0) { currentHealth = 0; }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke();
    }

    public void AddSouls(int amount)
    {
        currentSouls += amount;
        OnSoulsChanged?.Invoke(); // Сообщаем UI, что души изменились
        
        if (currentSouls >= soulsToNextLevel)
        {
            Debug.Log("Душ хватает! Подойдите к столу и нажмите E!");
        }
    }

    public void LevelUp()
    {
        if (currentSouls < soulsToNextLevel) return;

        currentSouls -= soulsToNextLevel;
        currentLevel++;
        soulsToNextLevel = Mathf.RoundToInt(soulsToNextLevel * 1.5f);
        
        OnSoulsChanged?.Invoke(); // Обновляем душ (т.к. мы их потратили)
        OnLevelChanged?.Invoke(); // Обновляем уровень
    }
}