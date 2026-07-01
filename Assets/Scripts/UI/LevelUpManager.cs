using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    void Awake() { Instance = this; uiPanel.SetActive(false); }

    public void OpenLevelUpUI()
    {
        if (_isLevelingUp) return;
        _isLevelingUp = true;
        uiPanel.SetActive(true);

        // Очищаем старые кнопки
        foreach (Transform child in cardButtonContainer) Destroy(child.gameObject);

        // Выбираем 3 случайные карты
        List<CardDataSO> chosenCards = GetRandomCards(3);

        // Создаем кнопки
        foreach (var card in chosenCards)
        {
            GameObject newButton = Instantiate(cardButtonPrefab, cardButtonContainer);
            // Меняем текст карты
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = card.cardName;
            
            // Прямо здесь вешаем событие нажатия без отдельного скрипта!
            newButton.GetComponent<Button>().onClick.AddListener(() => {
                ApplyCard(card);
            });
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
        PlayerStats stats = PlayerStats.Instance;
        if (stats != null) stats.LevelUp();
        ApplyEffectToPlayer(selectedCard);
        uiPanel.SetActive(false);
        _isLevelingUp = false;
    }

    private void ApplyEffectToPlayer(CardDataSO card)
    {
        PlayerController controller = FindAnyObjectByType<PlayerController>();
        PlayerStats stats = PlayerStats.Instance;
        if (controller == null || stats == null) return;

        switch (card.effectType)
        {
            case CardEffectType.DamageUp: stats.damage += card.value; break;
            case CardEffectType.MoveSpeedUp: controller.speed += card.value; break;
            case CardEffectType.FireRateUp: stats.fireRate = Mathf.Max(0.05f, stats.fireRate - card.value); break;
            case CardEffectType.MaxHealthUp: 
                stats.maxHealth += card.value; 
                stats.currentHealth += card.value; 
                break;
        }
    }
}