using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeData enemyUpgradeData;
    [SerializeField] private ProjectileShooter enemyShooter;

    [SerializeField] private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (enemyShooter && enemyUpgradeData)
        {
            enemyShooter.ApplyUpgradeData(enemyUpgradeData);
        }
        else
        {
            Debug.LogWarning("EnemyManager: Missing enemyShooter or enemyUpgradeData reference.");   
        }
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
