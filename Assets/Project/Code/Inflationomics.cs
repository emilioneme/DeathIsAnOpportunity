using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inflationomics : MonoBehaviour
{
    [SerializeField] TimelineTracker tracker;
    [SerializeField] GameManager gameManager;

    [Header("UI References")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceText;

    [Header("Upgrade Settings")]
    [SerializeField] private string upgradeId;      // Unique ID for this upgrade in TimelineTracker
    [SerializeField] private int basePrice = 15; // Price at level 1
    [SerializeField] private int priceIncrement = 5; // Price increase per level
    [SerializeField] private int maxLevel = 5;

    private int currentPrice;

    private int currentLevel = 0;

    private void Awake()
    {
        if (!upgradeButton) upgradeButton = GetComponent<Button>();

        // Load current level from TimelineTracker
        if (tracker.HasUpgrade(upgradeId)) currentLevel = Mathf.RoundToInt(tracker.GetUpgrade(upgradeId));
        UpdatePriceText();
        CheckPrice(currentPrice);
    }

    void CheckPrice(int price)
    {
        if (gameManager.soulCount <= price) 
        {
            upgradeButton.interactable=false;
            Debug.Log("you cannot afford this");
        }
        else
        {
            upgradeButton.interactable=true;
        }
    }

    public void OnUpgradeClicked()
    {
        if (currentLevel >= maxLevel)
            return; // Maxed out

        // Increase level
        currentLevel++;
        gameManager.soulCount -= (int)currentPrice;
        // Update upgradeTracker
        tracker.SetUpgrade(upgradeId, currentLevel);
        tracker.MarkEventCompleted($"Upgrade_{upgradeId}_Level{currentLevel}");

        UpdatePriceText();
    }

    private void UpdatePriceText()
    {
        if (currentLevel >= maxLevel)
        {
            priceText.text = "MAX";
            upgradeButton.interactable = false;
        }
        else
        {
            float nextPrice = basePrice + priceIncrement * currentLevel;
            priceText.text = $"{nextPrice}";
            currentPrice = (int)nextPrice;
            upgradeButton.interactable = true;
        }
    }
}
