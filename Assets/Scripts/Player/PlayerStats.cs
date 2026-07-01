using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public event Action OnHealthChanged;
    public event Action OnSoulsChanged;
    public event Action OnLevelChanged;

    public float maxHealth = 100f;
    public float currentHealth;

    public float damage = 10f;
    public float fireRate = 0.5f;

    public int currentLevel = 1;
    public int currentSouls = 0;
    public int soulsToNextLevel = 10; 

    void Awake() { Instance = this; }
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
        OnSoulsChanged?.Invoke();
        OnLevelChanged?.Invoke();
    }

    public void TakeDamage(float amount) { currentHealth -= amount; OnHealthChanged?.Invoke(); }
    public void Heal(float amount) { currentHealth = Mathf.Min(currentHealth + amount, maxHealth); OnHealthChanged?.Invoke(); }
    public void AddSouls(int amount)
    {
        currentSouls += amount;
        OnSoulsChanged?.Invoke();
    }
    public void LevelUp()
    {
        if (currentSouls < soulsToNextLevel) return;
        currentSouls -= soulsToNextLevel;
        currentLevel++;
        soulsToNextLevel = Mathf.RoundToInt(soulsToNextLevel * 1.5f);
        OnSoulsChanged?.Invoke();
        OnLevelChanged?.Invoke();
    }
}