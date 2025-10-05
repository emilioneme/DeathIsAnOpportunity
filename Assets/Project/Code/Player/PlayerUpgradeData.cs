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

    public float MoveSpeed => moveSpeed;
    public float MaxHorizontalSpeed => maxHorizontalSpeed;
    public float GroundAcceleration => groundAcceleration;
    public float AirAcceleration => airAcceleration;
    public float GroundLinearDrag => groundLinearDrag;
    public float AirLinearDrag => airLinearDrag;
    public float JumpImpulse => jumpImpulse;
    public int MaxAirJumps => maxAirJumps;
    public float CoyoteTime => coyoteTime;
    public float JumpBuffer => jumpBuffer;
    public float FallGravityMultiplier => fallGravityMultiplier;
    public float LowJumpGravityMultiplier => lowJumpGravityMultiplier;
    public float FireRate => fireRate;
    public float ProjectileSize => projectileSize;
    public float ProjectileDamage => projectileDamage;
    public float ProjectileSpeed => projectileSpeed;
    public float ProjectileLife => projectileLife;
    public int ProjectilesPerShot => projectilesPerShot;
    public float ProjectileSpreadRadius => projectileSpreadRadius;
    public float ProjectileAngleVariance => projectileAngleVariance;

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
