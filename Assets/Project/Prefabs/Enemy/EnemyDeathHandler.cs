using System.Collections;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    private Health health;

    public static event System.Action OnAnyEnemyDied;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDeath.AddListener(HandleDeath);
        else
            Debug.LogWarning("No Health component found on enemy.", gameObject);
    }

    private void HandleDeath()
    {
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
