using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerInputHub playerInputHub;
    [SerializeField] private ProjectileShooter shooter;
    // Update is called once per frame
    void Update()
    {
        shooter.isFiring = playerInputHub.AttackHeld;
    }
}
