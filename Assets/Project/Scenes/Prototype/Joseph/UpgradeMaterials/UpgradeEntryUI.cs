using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntryUI : MonoBehaviour
{
    [SerializeField]
    public string upgradeId;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public Button selectButton;

    public PlayerUpgradeData upgradeData;
    private ShopInteraction shop;

    public void Initialize(PlayerUpgradeData data, ShopInteraction shopInteraction)
    {
        shop = shopInteraction;

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() =>
        {
            shop.SelectUpgrade(upgradeId);
        });
    }

    public void SetInteractable(bool canBuy)
    {
        selectButton.interactable = canBuy;
        costText.color = canBuy ? Color.white : Color.gray;
    }
}
