using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathRoomTeleport : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SceneManagerFade sceneManager;
    private void Start()
    {
        health.OnDeath.AddListener(()=>sceneManager.LoadScene("DeathRoom"));
    }
}
