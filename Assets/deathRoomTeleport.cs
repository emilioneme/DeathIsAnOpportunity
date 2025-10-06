using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathRoomTeleport : MonoBehaviour
{
    [SerializeField] private Health health;
    private void Start()
    {
        health.OnDeath.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("DeathRoom"));
    }
}
