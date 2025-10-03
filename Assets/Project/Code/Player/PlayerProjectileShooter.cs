using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHub))]
public class PlayerProjectileShooter : MonoBehaviour
{
    private const int MaxProjectiles = 300;

    [Header("References")]
    [SerializeField] private PlayerInputHub inputHub;
    [SerializeField] private Transform projectileExit;
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

    private readonly Queue<Projectile> projectilePool = new();
    private readonly List<Projectile> activeProjectiles = new();
    private int createdProjectiles;
    private float fireCooldown;

    void Awake()
    {
        if (!inputHub)
            inputHub = GetComponent<PlayerInputHub>();

        if (!projectileExit)
            projectileExit = transform;
    }

    void Update()
    {
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        if (inputHub && inputHub.AttackHeld)
            TryFire(projectileExit.forward);
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
        if (!projectilePrefab || !projectileExit)
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
        Transform spawnPoint = projectileExit ? projectileExit : transform;
        Vector3[] offsets = BuildSpreadOffsets(Mathf.Max(1, projectilesPerShot), projectileSpreadRadius);
        Quaternion projectileRotation = Quaternion.LookRotation(fireDirection, spawnPoint.up);
        bool firedAny = false;

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 offset = offsets[i];
            Vector3 worldOffset = spawnPoint.right * offset.x + spawnPoint.up * offset.y;
            Vector3 spawnPosition = spawnPoint.position + worldOffset;
            Projectile projectileInstance = GetProjectile(spawnPosition, projectileRotation);

            if (!projectileInstance)
                break;

            firedAny = true;
            projectileInstance.Initialize(projectileDamage, projectileSpeed, projectileLife, projectilePath, projectileSize, gameObject);
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
