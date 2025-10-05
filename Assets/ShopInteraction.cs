using System.Collections.Generic;
using UnityEngine;



public class ShopInteraction : MonoBehaviour, IInteractable
{
    public GameManager GameManager;

    [Header("References")]
    [SerializeField] private GameObject shopUI;                 // The UI Panel (Canvas child)
    [SerializeField] private PlayerMovementRB playerMovement;   // Player movement controller
    [SerializeField] private PlayerCameraLook playerCameraLook; // Camera look controller

    private bool isShopOpen = false;
    private PlayerUpgradeData upgradeData;

    private void Awake()
    {
        GameManager = GetComponent<GameManager>();
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

    public void SelectUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "moveSpeed":
                if (upgradeData.moveSpeed < 100) 
                    upgradeData.moveSpeed += 18.6f;
                break;

            case "groundAcceleration":
                if (upgradeData.groundAcceleration < 200) 
                    upgradeData.groundAcceleration += 32;
                break;

            case "airAcceleration":
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
        }
        upgradeData.NotifyChanged();
    }
}
