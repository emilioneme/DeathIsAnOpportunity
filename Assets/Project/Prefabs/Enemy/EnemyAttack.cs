using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private ProjectileShooter shooter;
    // Update is called once per frame
    
    void Update()
    {
        shooter.isFiring = gameObject.HasLineOfSight(target, 10000);
    }
}
