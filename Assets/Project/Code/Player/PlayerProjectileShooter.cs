using UnityEngine;

[RequireComponent(typeof(PlayerInputHub))]
public class PlayerProjectileShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHub inputHub;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectilePrefab;

    [Header("Combat Values")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float projectileSize = 1f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private ProjectilePathType projectilePath = ProjectilePathType.Normal;
    [SerializeField] private float projectileLife = 3f;
    [SerializeField] private int projectilesPerShot = 1;
    [SerializeField] private float projectileSpreadRadius = 0.5f;

    private float fireCooldown;

    void Awake()
    {
        if (!inputHub)
            inputHub = GetComponent<PlayerInputHub>();

        if (!muzzle)
            muzzle = transform;
    }

    void Update()
    {
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        if (inputHub && inputHub.AttackHeld)
            TryFire(muzzle.forward);
    }

    public void ApplyUpgradeData(PlayerUpgradeData data)
    {
        if (!data)
            return;

        fireRate = data.FireRate;
        projectileSize = data.ProjectileSize;
        projectileDamage = data.ProjectileDamage;
        projectileSpeed = data.ProjectileSpeed;
        projectilePath = data.ProjectilePath;
        projectileLife = data.ProjectileLife;
        projectilesPerShot = Mathf.Max(1, data.ProjectilesPerShot);
        projectileSpreadRadius = data.ProjectileSpreadRadius;
    }

    public void TryFire(Vector3 fireDirection)
    {
        if (!projectilePrefab || !muzzle)
            return;

        if (fireCooldown > 0f)
            return;

        FireProjectiles(fireDirection.normalized);

        fireCooldown = fireRate > 0f ? 1f / fireRate : 0f;
    }

    private void FireProjectiles(Vector3 fireDirection)
    {
        Transform spawnPoint = muzzle ? muzzle : transform;
        Vector3[] offsets = BuildSpreadOffsets(Mathf.Max(1, projectilesPerShot), projectileSpreadRadius);
        Quaternion projectileRotation = Quaternion.LookRotation(fireDirection, spawnPoint.up);

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 offset = offsets[i];
            Vector3 worldOffset = spawnPoint.right * offset.x + spawnPoint.up * offset.y;
            Projectile projectileInstance = Instantiate(projectilePrefab, spawnPoint.position + worldOffset, projectileRotation);
            projectileInstance.Initialize(projectileDamage, projectileSpeed, projectileLife, projectilePath, projectileSize, gameObject);
        }
    }

    private Vector3[] BuildSpreadOffsets(int count, float radius)
    {
        if (count <= 1 || radius <= 0f)
            return new[] { Vector3.zero };

        Vector3[] offsets = new Vector3[count];
        float angleStep = Mathf.PI * 2f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            offsets[i] = new Vector3(x, y, 0f);
        }

        return offsets;
    }
}
