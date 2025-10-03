using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private static readonly List<Collider> ActiveProjectiles = new();

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider projectileCollider;

    private float damage;
    private float lifeTime;
    private ProjectilePathType pathType;
    private GameObject owner;
    private PlayerProjectileShooter poolOwner;

    public float Damage => damage;

    void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();

        if (!projectileCollider)
            projectileCollider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        RegisterProjectileCollider();
    }

    void OnDisable()
    {
        UnregisterProjectileCollider();
        CancelInvoke(nameof(HandleLifeComplete));

        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Initialize(float damageValue, float speedValue, float lifeDuration, ProjectilePathType path, float size, GameObject instigator)
    {
        damage = damageValue;
        lifeTime = lifeDuration;
        pathType = path;
        owner = instigator;

        if (rb)
        {
            rb.useGravity = pathType == ProjectilePathType.Normal;
            rb.linearVelocity = transform.forward * speedValue;
        }

        float clampedSize = Mathf.Max(0.01f, size);
        transform.localScale = Vector3.one * clampedSize;

        CancelInvoke(nameof(HandleLifeComplete));

        if (lifeTime > 0f)
            Invoke(nameof(HandleLifeComplete), lifeTime);
    }

    public void SetPoolOwner(PlayerProjectileShooter shooter)
    {
        poolOwner = shooter;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other)
            return;

        if (owner && other.transform.root == owner.transform)
            return;

        Deactivate();
    }

    private void HandleLifeComplete()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        if (poolOwner)
        {
            poolOwner.ReclaimProjectile(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void RegisterProjectileCollider()
    {
        if (!projectileCollider)
            return;

        for (int i = 0; i < ActiveProjectiles.Count; i++)
        {
            Collider otherCollider = ActiveProjectiles[i];
            if (!otherCollider)
                continue;

            Physics.IgnoreCollision(projectileCollider, otherCollider, true);
        }

        if (!ActiveProjectiles.Contains(projectileCollider))
            ActiveProjectiles.Add(projectileCollider);
    }

    private void UnregisterProjectileCollider()
    {
        if (!projectileCollider)
            return;

        ActiveProjectiles.Remove(projectileCollider);
    }
}
