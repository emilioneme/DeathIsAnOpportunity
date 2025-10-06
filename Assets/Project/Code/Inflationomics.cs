using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inflationomics : MonoBehaviour
{
    [SerializeField] TimelineTracker tracker;

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
        //currentLevel = Mathf.RoundToInt(tracker.GetUpgrade(upgradeId));
        UpdatePriceText();

    }

    public void OnUpgradeClicked()
    {
        Debug.Log("Clicked");
        if (currentLevel >= maxLevel)
            return; // Maxed out

        // Increase level
        currentLevel++;

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
            priceText.text = $"Price: {nextPrice}";
            upgradeButton.interactable = true;
        }
    }
}
