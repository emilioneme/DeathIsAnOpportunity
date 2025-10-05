using System.Collections.Generic;
using UnityEngine;



public class ShopInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private List<PlayerUpgradeData> availableUpgrades;
    [Header("References")]
    [SerializeField] private GameObject shopUI;                 // The UI Panel (Canvas child)
    [SerializeField] private PlayerMovementRB playerMovement;   // Player movement controller
    [SerializeField] private PlayerCameraLook playerCameraLook; // Camera look controller

    private bool isShopOpen = false;

    public void Interact()
    {
        if (!isShopOpen)
            OpenShop();
        else
            CloseShop();
    }

    private void OpenShop()
    {
        // Show the shop UI
        shopUI.SetActive(true);
        isShopOpen = true;

        // Freeze player + camera
        playerMovement.CanMove = false;
        playerCameraLook.SetCursorLock(false);

        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        isShopOpen = false;

        // Unfreeze player + relock camera
        playerMovement.CanMove = true;
        playerCameraLook.SetCursorLock(true);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Close with Escape while shop is open
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
}
