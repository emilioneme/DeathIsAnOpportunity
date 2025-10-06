using System.Collections.Generic;
using UnityEngine;



public class ShopInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameManager GameManager;

    [Header("References")]
    [SerializeField] private GameObject shopUI;                 // The UI Panel (Canvas child)
    [SerializeField] private PlayerMovementRB playerMovement;   // Player movement controller
    [SerializeField] private PlayerCameraLook playerCameraLook; // Camera look controller
    [SerializeField] private PlayerUI playerUI;

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

    }

    public void CloseShop()
    {
        playerUI.interacted = false;
        shopUI.SetActive(false);
        isShopOpen = false;

        // Unfreeze player + relock camera
        playerMovement.CanMove = true;
        playerCameraLook.SetCursorLock(true);

    }

    private void Update()
    {
        // Close with Escape while shop is open
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void SelectUpgrade(string upgrade)
    {
        Debug.Log(upgrade);
        Debug.Log(upgradeData.moveSpeed);
        switch (upgrade)
        {
            case "moveSpeed":
                Debug.Log("It's Happening!!!");
                if (upgradeData.moveSpeed < 100) 
                    upgradeData.moveSpeed += 18.6f;
                break;

            case "groundAccel":
                if (upgradeData.groundAcceleration < 200) 
                    upgradeData.groundAcceleration += 32;
                break;

            case "airAccel":
                if (upgradeData.airAcceleration < 150) 
                    upgradeData.airAcceleration += 28;
                break;

            case "jumpImpulse":
                if (upgradeData.jumpImpulse < 15) 
                    upgradeData.jumpImpulse += 2;
                break;

            case "maxAirJumps":
                if (upgradeData.maxAirJumps < 10) 
                    upgradeData.maxAirJumps += 1;
                break;
            case "fireRate":
                if (upgradeData.fireRate < 20f)
                    upgradeData.fireRate += 3.6f;
                break;

            case "projectileSpeed":
                if (upgradeData.projectileSpeed < 100f)
                    upgradeData.projectileSpeed += 16f;
                break;

            case "projectileDamage":
                if (upgradeData.projectileDamage < 1000f)
                    upgradeData.projectileDamage += 198f;
                break;

            case "projectilesPerShot":
                if (upgradeData.projectilesPerShot < 20)
                    upgradeData.projectilesPerShot += 4;
                break;

            case "projectileAngleVariance":
                if (upgradeData.projectileAngleVariance < 10f)
                    upgradeData.projectileAngleVariance += 1f;
                break;
            default:
                Debug.Log("FUBAR");
                break;
        }

        upgradeData.NotifyChanged();
    }
}
