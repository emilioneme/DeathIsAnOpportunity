using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeData enemyUpgradeData;
    [SerializeField] private ProjectileShooter enemyShooter;

    [SerializeField] private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyShooter.ApplyUpgradeData(enemyUpgradeData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerProjectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            health.TakeDamage(projectile.Damage);
        }
    }
}
