using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private ProjectileShooter projectileShooter;
    [SerializeField] private PlayerInputHub playerInputHub;

    private void Update()
    {
        projectileShooter.isFiring = playerInputHub && playerInputHub.AttackHeld;

    }
}
