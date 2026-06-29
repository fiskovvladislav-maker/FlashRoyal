using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Data/Card")]
public class CardDataSO : ScriptableObject
{
    public string cardName;        // Например, "Туз Пик"
    public Sprite icon;            // Картинка карты (потом добавите)
    
    [Header("Effect")]
    public CardEffectType effectType; // Тип усиления
    public float value;               // Значение усиления (например, +1 к урону, +10% к скорости)
}

public enum CardEffectType
{
    DamageUp,       // + Урон
    MoveSpeedUp,    // + Скорость передвижения
    FireRateUp,     // + Скорострельность
    DashDistanceUp, // + Дальность рывка
    MaxHealthUp     // + Макс. здоровье
}