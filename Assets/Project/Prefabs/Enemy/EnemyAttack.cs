using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] public GameObject target;
    [SerializeField] private ProjectileShooter shooter;
    // Update is called once per frame
    
    void Update()
    {
        if(target == null) return;
        shooter.isFiring = gameObject.HasLineOfSight(target, 10000);
    }
}
