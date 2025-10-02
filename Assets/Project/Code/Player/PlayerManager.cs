using UnityEngine;

[RequireComponent(typeof(PlayerMovementRB))]
public class PlayerManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerUpgradeData upgradeData;

    [Header("References")]
    [SerializeField] private PlayerMovementRB movement;
    [SerializeField] private PlayerProjectileShooter shooter;

    public PlayerUpgradeData UpgradeData => upgradeData;
    public PlayerProjectileShooter Shooter => shooter;

    void Awake()
    {
        CacheComponents();
        RefreshUpgradeData();
    }

    void OnEnable()
    {
        SubscribeToUpgradeData(upgradeData);
        RefreshUpgradeData();
    }

    void OnDisable()
    {
        UnsubscribeFromUpgradeData(upgradeData);
    }

    void OnValidate()
    {
        CacheComponents();

        if (!Application.isPlaying)
            RefreshUpgradeData();
    }

    public void ApplyUpgradeData(PlayerUpgradeData data)
    {
        if (data == null)
            return;

        if (upgradeData == data)
        {
            RefreshUpgradeData();
            return;
        }

        UnsubscribeFromUpgradeData(upgradeData);
        upgradeData = data;
        SubscribeToUpgradeData(upgradeData);
        RefreshUpgradeData();
    }

    private void CacheComponents()
    {
        if (!movement)
            movement = GetComponent<PlayerMovementRB>();

        if (!shooter)
            shooter = GetComponent<PlayerProjectileShooter>();
    }

    private void RefreshUpgradeData()
    {
        if (!upgradeData)
            return;

        if (movement)
            movement.ApplyUpgradeData(upgradeData);

        if (shooter)
            shooter.ApplyUpgradeData(upgradeData);
    }

    private void SubscribeToUpgradeData(PlayerUpgradeData data)
    {
        if (!data)
            return;

        data.OnChanged -= HandleUpgradeDataChanged;
        data.OnChanged += HandleUpgradeDataChanged;
    }

    private void UnsubscribeFromUpgradeData(PlayerUpgradeData data)
    {
        if (!data)
            return;

        data.OnChanged -= HandleUpgradeDataChanged;
    }

    private void HandleUpgradeDataChanged(PlayerUpgradeData data)
    {
        if (data == upgradeData)
            RefreshUpgradeData();
    }
}
