using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntryUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public Button selectButton;

    private PlayerUpgradeData upgradeData;
    private ShopInteraction shop;

    public void Initialize(PlayerUpgradeData data, ShopInteraction shopInteraction)
    {
        upgradeData = data;
        shop = shopInteraction;

        nameText.text = data.name;
        //descriptionText.text = data.Description; // if none, just use data.name
        costText.text = "Buy";

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() =>
        {
            //shop.SelectUpgrade(upgradeData);
        });
    }

    public void SetInteractable(bool canBuy)
    {
        selectButton.interactable = canBuy;
        costText.color = canBuy ? Color.white : Color.gray;
    }
}
