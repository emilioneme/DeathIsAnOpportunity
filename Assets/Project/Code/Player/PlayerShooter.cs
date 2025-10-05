using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private ProjectileShooter projectileShooter;
    [SerializeField] private PlayerInputHub playerInputHub;
    [SerializeField] private Animator animator;
    private bool isHoldingAttack = false;
    private bool isShooting = false;

    public void OnShootAnimationEnd()
    {
        projectileShooter.isFiring = true;
        Debug.Log("Shooting");
        // Example: reset shooting flag
    }

    private void Start()
    {
        playerInputHub.OnFingerPressed += ()=>animator.SetTrigger("Finger");
    }

    private void Update()
    {
        isHoldingAttack = playerInputHub && playerInputHub.AttackHeld;
        if (projectileShooter.CanFire)
        {
            if (!isShooting && isHoldingAttack && !projectileShooter.isFiring)
            {
                animator.SetTrigger("Shoot");
                isShooting =  true;
            } 
        }
        else
        {
            projectileShooter.isFiring = false;
            isShooting  = false;
        }

    }
}
