using System.Collections;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    private Health health;

    public static event System.Action OnAnyEnemyDied;

    private void Awake()
    {
        Debug.Log("EnemyDeathHandler.Awake called", gameObject);
        health = GetComponent<Health>();

        if (health != null)
        {
            Debug.Log("Health component found", gameObject);
            health.OnDeath.AddListener(HandleDeath);
            Debug.Log("Listener added to Health.OnDeath", gameObject);
        }
        else
        {
            Debug.LogWarning("No Health component found on enemy.", gameObject);
        }
    }

    private void HandleDeath()
    {
        Debug.Log("On Death Handled");
        OnAnyEnemyDied?.Invoke();
        StartCoroutine(DestroyAfterDelay(0.5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDeath.RemoveListener(HandleDeath);
    }
}
