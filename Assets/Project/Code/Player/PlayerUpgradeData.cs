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
    [SerializeField] private ProjectilePathType projectilePath = ProjectilePathType.Normal;
    [SerializeField] private float projectileLife = 3f;
    [SerializeField] private int projectilesPerShot = 1;
    [SerializeField] private float projectileSpreadRadius = 0.5f;

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
    public ProjectilePathType ProjectilePath => projectilePath;
    public float ProjectileLife => projectileLife;
    public int ProjectilesPerShot => Mathf.Max(1, projectilesPerShot);
    public float ProjectileSpreadRadius => Mathf.Max(0f, projectileSpreadRadius);

    public void NotifyChanged()
    {
        OnChanged?.Invoke(this);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        NotifyChanged();
    }
#endif
}
