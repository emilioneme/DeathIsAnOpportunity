using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inflationomics : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text priceText;

    [Header("Upgrade Settings")]
    [SerializeField] private string upgradeId;      // Unique ID for this upgrade in TimelineTracker
    [SerializeField] private float basePrice = 100f; // Price at level 1
    [SerializeField] private float priceIncrement = 50f; // Price increase per level
    [SerializeField] private int maxLevel = 5;

    private int currentLevel = 0;

    private void Awake()
    {
        if (!upgradeButton) upgradeButton = GetComponent<Button>();

        // Load current level from TimelineTracker
        currentLevel = Mathf.RoundToInt(TimelineTracker.Instance.GetUpgrade(upgradeId));
        UpdatePriceText();

    }

    public void OnUpgradeClicked()
    {
        if (currentLevel >= maxLevel)
            return; // Maxed out

        // Increase level
        currentLevel++;

        // Update upgradeTracker
        TimelineTracker.Instance.SetUpgrade(upgradeId, currentLevel);
        TimelineTracker.Instance.MarkEventCompleted($"Upgrade_{upgradeId}_Level{currentLevel}");

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
            priceText.text = $"Price: {nextPrice}";
            upgradeButton.interactable = true;
        }
    }
}
