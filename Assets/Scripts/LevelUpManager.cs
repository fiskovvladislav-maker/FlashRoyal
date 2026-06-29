using UnityEngine;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [Header("Data")]
    public List<CardDataSO> allCards; 

    [Header("UI")]
    public GameObject uiPanel;        
    public GameObject cardButtonPrefab; 
    public Transform cardButtonContainer; 

    private bool _isLevelingUp = false;

    void Awake() 
    { 
        Instance = this; 
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    public void OpenLevelUpUI()
    {
        if (_isLevelingUp) return;
        _isLevelingUp = true;
        uiPanel.SetActive(true);

        foreach (Transform child in cardButtonContainer) Destroy(child.gameObject);

        List<CardDataSO> chosenCards = GetRandomCards(3);

        foreach (var card in chosenCards)
        {
            GameObject newButton = Instantiate(cardButtonPrefab, cardButtonContainer);
            newButton.GetComponent<CardButtonUI>().Setup(card, this);
        }
    }

    private List<CardDataSO> GetRandomCards(int count)
    {
        List<CardDataSO> randomList = new List<CardDataSO>();
        List<CardDataSO> tempPool = new List<CardDataSO>(allCards);

        for (int i = 0; i < count && tempPool.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempPool.Count);
            randomList.Add(tempPool[randomIndex]);
            tempPool.RemoveAt(randomIndex);
        }
        return randomList;
    }

    public void ApplyCard(CardDataSO selectedCard)
    {
        Debug.Log($"Игрок выбрал карту: {selectedCard.cardName}!");
        PlayerStats stats = PlayerStats.Instance;
        if (stats != null) stats.LevelUp();
        ApplyEffectToPlayer(selectedCard);
        uiPanel.SetActive(false);
        _isLevelingUp = false;
    }

    private void ApplyEffectToPlayer(CardDataSO card)
    {
        // ИЗМЕНЕНИЕ ЗДЕСЬ: FindAnyObjectByType вместо FindFirstObjectByType
        PlayerController controller = FindAnyObjectByType<PlayerController>();
        PlayerStats stats = PlayerStats.Instance;

        if (controller == null || stats == null) return;

        switch (card.effectType)
        {
            case CardEffectType.DamageUp:
                stats.damage += card.value;
                Debug.Log($"Урон увеличен на {card.value}");
                break;
            case CardEffectType.MoveSpeedUp:
                controller.speed += card.value;
                Debug.Log($"Скорость движения увеличена на {card.value}");
                break;
            case CardEffectType.FireRateUp:
                stats.fireRate -= card.value;
                Debug.Log($"Скорострельность увеличена");
                break;
            case CardEffectType.MaxHealthUp:
                stats.maxHealth += card.value;
                stats.currentHealth += card.value;
                Debug.Log($"Макс. здоровье увеличено до {stats.maxHealth}");
                break;
        }
    }
}