using UnityEngine;
using UnityEngine.UI;
using TMPro; // Подключаем пространство имен для TextMeshPro

public class CardButtonUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private CardDataSO _cardData;
    private LevelUpManager _manager;

    public void Setup(CardDataSO cardData, LevelUpManager manager)
    {
        _cardData = cardData;
        _manager = manager;

        titleText.text = cardData.cardName;
        // Генерируем описание в зависимости от типа карты
        descriptionText.text = GetDescription(cardData);

        // Убираем старые подписки на нажатие, чтобы не было багов, и добавляем новую
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCardSelected);
    }

    private string GetDescription(CardDataSO card)
    {
        switch (card.effectType)
        {
            case CardEffectType.DamageUp:          return $"Увеличивает урон на {card.value}";
            case CardEffectType.MoveSpeedUp:       return $"Увеличивает скорость на {card.value}";
            case CardEffectType.FireRateUp:        return $"Ускоряет стрельбу на {card.value}";
            case CardEffectType.MaxHealthUp:       return $"Увеличивает здоровье на {card.value}";
            default: return "Таинственная карта";
        }
    }

    private void OnCardSelected()
    {
        // Сообщаем менеджеру, какую карту выбрали
        _manager.ApplyCard(_cardData);
    }
}