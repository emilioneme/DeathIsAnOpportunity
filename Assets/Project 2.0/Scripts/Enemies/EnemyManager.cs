using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private PlayerUpgradeData enemyUpgradeData;
    [SerializeField] private ProjectileShooter enemyShooter;

    [SerializeField] private Health health;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("it's bleak");
            return;
        }
        else Debug.Log("we're so back");

        
        //health.OnDeath.AddListener(SoulCount);
    }
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
    
    void SoulCount()
    {
        gameManager.soulCount++;
        Debug.Log(gameManager.soulCount);
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
            SoulCount();
        }
    }
}
