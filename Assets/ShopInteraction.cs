using System.Collections.Generic;
using UnityEngine;



public class ShopInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameManager GameManager;

    [Header("References")]
    [SerializeField] private GameObject shopUI;                 // The UI Panel (Canvas child)
    [SerializeField] private PlayerMovementRB playerMovement;   // Player movement controller
    [SerializeField] private PlayerCameraLook playerCameraLook; // Camera look controller

    private bool isShopOpen = false;
    private PlayerUpgradeData upgradeData;

    private void Awake()
    {
        upgradeData = GameManager.upgradeData;
    }
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

    public void SelectUpgrade(int upgrade)
    {
        switch (upgrade)
        {
            case 1:
                if (upgradeData.moveSpeed < 100) 
                    upgradeData.moveSpeed += 18.6f;
                break;

            case 2:
                if (upgradeData.groundAcceleration < 200) 
                    upgradeData.groundAcceleration += 32;
                break;

            case 3:
                if (upgradeData.airAcceleration < 150) 
                    upgradeData.airAcceleration += 28;
                break;

            case 4:
                if (upgradeData.jumpImpulse < 15) 
                    upgradeData.jumpImpulse += 2;
                break;

            case 5:
                if (upgradeData.maxAirJumps < 10) 
                    upgradeData.maxAirJumps += 1;
                break;
            case 6:
                if (upgradeData.fireRate < 20f)
                    upgradeData.fireRate += 3.6f;
                break;

            case 7:
                if (upgradeData.projectileSpeed < 100f)
                    upgradeData.projectileSpeed += 16f;
                break;

            case 8:
                if (upgradeData.projectileDamage < 1000f)
                    upgradeData.projectileDamage += 198f;
                break;

            case 9:
                if (upgradeData.projectilesPerShot < 20)
                    upgradeData.projectilesPerShot += 4;
                break;

            case 10:
                if (upgradeData.projectileAngleVariance < 10f)
                    upgradeData.projectileAngleVariance += 1f;
                break;
        }
        upgradeData.NotifyChanged();
    }
}
