using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHub))]
public class PlayerProjectileShooter : MonoBehaviour
{
    private const int MaxProjectiles = 500;

    [Header("References")]
    [SerializeField] private PlayerInputHub inputHub;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectilePrefab;

    private float fireRate = 2f;
    private float projectileSize = 1f;
    private float projectileDamage = 10f;
    private float projectileSpeed = 20f;
    private float projectileLife = 3f;
    private int projectilesPerShot = 1;
    private float projectileSpreadRadius = 0.5f;
    private float projectileAngleVariance = 5f;

    private readonly Queue<Projectile> projectilePool = new();
    private readonly List<Projectile> activeProjectiles = new();
    private int createdProjectiles;
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
        projectileLife = data.ProjectileLife;
        projectilesPerShot = Mathf.Max(1, data.ProjectilesPerShot);
        projectileSpreadRadius = data.ProjectileSpreadRadius;
        projectileAngleVariance = data.ProjectileAngleVariance;
    }

    public void TryFire(Vector3 fireDirection)
    {
        if (!projectilePrefab || !muzzle)
            return;

        if (fireCooldown > 0f)
            return;

        if (FireProjectiles(fireDirection.normalized))
            fireCooldown = fireRate > 0f ? 1f / fireRate : 0f;
    }

    public void ReclaimProjectile(Projectile projectile)
    {
        if (!projectile)
            return;

        if (activeProjectiles.Remove(projectile))
        {
            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    private bool FireProjectiles(Vector3 fireDirection)
    {
        Transform spawnPoint = muzzle ? muzzle : transform;
        int count = Mathf.Max(1, projectilesPerShot);
        bool firedAny = false;

        Quaternion baseRotation = Quaternion.LookRotation(fireDirection, spawnPoint.up);

        for (int i = 0; i < count; i++)
        {
            Vector2 spreadOffset = SampleSpreadOffset(count, projectileSpreadRadius);
            Vector3 worldOffset = spawnPoint.right * spreadOffset.x + spawnPoint.up * spreadOffset.y;
            Vector3 spawnPosition = spawnPoint.position + worldOffset;

            Quaternion randomizedRotation = GetRandomizedRotation(baseRotation, projectileAngleVariance);
            Projectile projectileInstance = GetProjectile(spawnPosition, randomizedRotation);

            if (!projectileInstance)
                break;

            firedAny = true;
            projectileInstance.Initialize(projectileDamage, projectileSpeed, projectileLife, projectileSize, gameObject);
        }

        return firedAny;
    }

    private Projectile GetProjectile(Vector3 position, Quaternion rotation)
    {
        while (projectilePool.Count > 0 && !projectilePool.Peek())
            projectilePool.Dequeue();

        if (projectilePool.Count == 0 && createdProjectiles >= MaxProjectiles)
            ForceRecycleOldestProjectile();

        Projectile projectileInstance = null;

        if (projectilePool.Count > 0)
        {
            projectileInstance = projectilePool.Dequeue();
            projectileInstance.transform.SetPositionAndRotation(position, rotation);
            projectileInstance.gameObject.SetActive(true);
        }
        else if (createdProjectiles < MaxProjectiles)
        {
            projectileInstance = Instantiate(projectilePrefab, position, rotation);
            projectileInstance.SetPoolOwner(this);
            createdProjectiles++;
        }

        if (projectileInstance)
        {
            projectileInstance.SetPoolOwner(this);
            activeProjectiles.Add(projectileInstance);
        }

        return projectileInstance;
    }

    private void ForceRecycleOldestProjectile()
    {
        for (int i = 0; i < activeProjectiles.Count; i++)
        {
            Projectile projectile = activeProjectiles[i];
            activeProjectiles.RemoveAt(i);

            if (!projectile)
                continue;

            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile);
            break;
        }
    }

    private Vector2 SampleSpreadOffset(int totalCount, float radius)
    {
        if (radius <= 0f)
            return Vector2.zero;

        if (totalCount <= 1)
            return Random.insideUnitCircle * (radius * 0.25f);

        return Random.insideUnitCircle * radius;
    }

    private Quaternion GetRandomizedRotation(Quaternion baseRotation, float angleVariance)
    {
        if (angleVariance <= 0f)
            return baseRotation;

        Vector3 localUp = baseRotation * Vector3.up;
        Vector3 localRight = baseRotation * Vector3.right;

        float yaw = Random.Range(-angleVariance, angleVariance);
        float pitch = Random.Range(-angleVariance, angleVariance);

        Quaternion yawRotation = Quaternion.AngleAxis(yaw, localUp);
        Quaternion pitchRotation = Quaternion.AngleAxis(pitch, localRight);
        Quaternion randomizedRotation = yawRotation * pitchRotation * baseRotation;

        return randomizedRotation;
    }
}
