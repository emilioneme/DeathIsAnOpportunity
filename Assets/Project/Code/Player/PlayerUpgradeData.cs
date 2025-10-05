using System;
using UnityEngine;

public enum ProjectilePathType
{
    Normal,
    Straight
}

[CreateAssetMenu(fileName = "PlayerUpgradeData", menuName = "Player/Upgrade Data")]
public class PlayerUpgradeData : ScriptableObject
{
    private const float MoveToMaxSpeedRatio = 1.5f;
    private const float BaseProjectileSize = 1f;
    private const float BaseProjectileSpread = .1f;

    public event Action<PlayerUpgradeData> OnChanged;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float maxHorizontalSpeed = 10f;
    [SerializeField] private float groundAcceleration = 40f;
    [SerializeField] private float airAcceleration = 15f;
    [SerializeField] private float groundLinearDrag = 6f;
    [SerializeField] private float airLinearDrag = 0.5f;

    [Header("Jumping")]
    [SerializeField] private float jumpImpulse = 7.5f;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBuffer = 0.12f;
    [SerializeField] private float fallGravityMultiplier = 2.0f;
    [SerializeField] private float lowJumpGravityMultiplier = 2.5f;

    [Header("Combat")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float projectileSize = 1f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileLife = 3f;
    [SerializeField] private int projectilesPerShot = 1;
    [SerializeField] private float projectileSpreadRadius = 0.5f;
    [SerializeField] private float projectileAngleVariance = 5f;

    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float GroundLinearDrag => groundLinearDrag; // 
    public float MaxHorizontalSpeed => maxHorizontalSpeed;     // -------- DON'T INCLUDE IN SHOP ------------- //
    public float CoyoteTime => coyoteTime;     // -------- DON'T INCLUDE IN SHOP ------------- //
    public float JumpBuffer => jumpBuffer;    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float FallGravityMultiplier => fallGravityMultiplier;    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float LowJumpGravityMultiplier => lowJumpGravityMultiplier;    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float ProjectileSize => projectileSize;    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float ProjectileSpreadRadius => projectileSpreadRadius;    // -------- DON'T INCLUDE IN SHOP ------------- //
    public float AirLinearDrag => airLinearDrag;     // -------- DON'T INCLUDE IN SHOP ------------- //




    // -------------- Include in Shop -------------- //
    public float MoveSpeed => moveSpeed; // min - 7.5; extreme cap - 100 ; Make Expensive
    public float GroundAcceleration => groundAcceleration; // min - 40; extreme cap - 200; Make Expensive
    public float AirAcceleration => airAcceleration; // min - 10; max - 150; Make Expensive
    public float JumpImpulse => jumpImpulse; // min - 5; max - 15
    public int MaxAirJumps => maxAirJumps; // min - 0; max - 10
    public float FireRate => fireRate; // min - 0.5; max - 20
    public float ProjectileDamage => projectileDamage; // min - 1; max - 1000
    public float ProjectileSpeed => projectileSpeed; // min - 10; max - 100
    public int ProjectilesPerShot => projectilesPerShot; // min - 1; max - 20
    public float ProjectileAngleVariance => projectileAngleVariance; // min - 10; max - 0

    public float ProjectileLife => projectileLife; // min - 2; max - 10; (potentially just keep it at 10)


    public void NotifyChanged()
    {
        EnforceDerivedRelationships();
        OnChanged?.Invoke(this);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        EnforceDerivedRelationships();
        NotifyChanged();
    }
#endif

    private void EnforceDerivedRelationships()
    {
        if (moveSpeed < 0f)
            moveSpeed = 0f;

        float targetMaxSpeed = moveSpeed * MoveToMaxSpeedRatio;
        if (!Mathf.Approximately(maxHorizontalSpeed, targetMaxSpeed))
            maxHorizontalSpeed = targetMaxSpeed;

        projectilesPerShot = Mathf.Max(1, projectilesPerShot);

        float projectileCountScale = Mathf.Sqrt(projectilesPerShot);
        float targetSize = Mathf.Max(0.05f, BaseProjectileSize / projectileCountScale);
        float targetSpread = Mathf.Max(0f, BaseProjectileSpread * projectileCountScale);

        if (!Mathf.Approximately(projectileSize, targetSize))
            projectileSize = targetSize;

        if (!Mathf.Approximately(projectileSpreadRadius, targetSpread))
            projectileSpreadRadius = targetSpread;

        if (projectileAngleVariance < 0f)
            projectileAngleVariance = 0f;
    }
}
