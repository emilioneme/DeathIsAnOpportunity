using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Gameplay/Health")]
public class Health : MonoBehaviour
{
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField] private bool startAtMax = true;
    [SerializeField] private bool invulnerable = false;
    [SerializeField] GameObject DeathParticles;
    [SerializeField] GameObject DamageParticles;

    [SerializeField, Tooltip("When true, healing cannot exceed Max Health.")]

    private bool clampOverheal = true;

    // Hidden: shown by the custom inspector as a bar + slider
    [SerializeField] private float currentHealth = 100f;

    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = Mathf.Max(1f, value);
            if (clampOverheal && currentHealth > maxHealth)
                currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0f;
    public bool Invulnerable { get => invulnerable; set => invulnerable = value; }

    [System.Serializable] public class HealthEvent : UnityEvent<float, float> { } // (current, max)
    public HealthEvent OnHealthChanged = new HealthEvent();
    public UnityEvent OnDamaged = new UnityEvent();
    public UnityEvent OnHealed = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();

    private Coroutine dotRoutine;

    private void Awake()
    {

        if (startAtMax) currentHealth = maxHealth;
        else currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        if (startAtMax && !Application.isPlaying)
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
#endif

    /// <summary>Apply instant damage. Returns the actual damage done.</summary>
    public float TakeDamage(float amount)
    {
        if (amount <= 0f || invulnerable || IsDead) return 0f;

        float prev = currentHealth;
        currentHealth = Mathf.Max(0f, currentHealth - amount);

        if (currentHealth != prev)
        {
            OnDamaged?.Invoke();
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            if (DamageParticles != null)
                Destroy(Instantiate(DamageParticles, transform.position, Quaternion.identity), 3f);
        }

        if (currentHealth <= 0f)
        {
            Debug.Log($"OnDeath is {(OnDeath == null ? "null" : "not null")}, listeners: {(OnDeath != null ? OnDeath.GetPersistentEventCount() : 0)}", gameObject);
            OnDeath?.Invoke();
            if (DeathParticles != null)
            {
                Destroy(Instantiate(DeathParticles, transform.position, Quaternion.identity), 3f);
            }
        }
            

        return prev - currentHealth;
    }

    /// <summary>Heal instantly. Returns the actual health gained.</summary>
    public float Heal(float amount)
    {
        if (amount <= 0f || IsDead) return 0f;

        float prev = currentHealth;
        currentHealth = clampOverheal
            ? Mathf.Min(maxHealth, currentHealth + amount)
            : currentHealth + amount;

        if (currentHealth != prev)
        {
            OnHealed?.Invoke();
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        return currentHealth - prev;
    }

    /// <summary>Set health directly (optionally clamped).</summary>
    public void SetHealth(float value, bool clamp = true)
    {
        float prev = currentHealth;
        currentHealth = clamp ? Mathf.Clamp(value, 0f, maxHealth) : value;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (prev > 0f && currentHealth <= 0f)
            OnDeath?.Invoke();
    }

    /// <summary>
    /// Apply damage over time. Returns the running coroutine.
    /// dps: damage per second, duration: seconds, tickInterval: seconds between ticks.
    /// If a previous DoT is running, it is stopped/replaced.
    /// </summary>
    public Coroutine ApplyDamageOverTime(float dps, float duration, float tickInterval = 0.1f)
    {
        if (dps <= 0f || duration <= 0f) return null;
        if (dotRoutine != null) StopCoroutine(dotRoutine);
        dotRoutine = StartCoroutine(DamageOverTime(dps, duration, tickInterval));
        return dotRoutine;
    }

    /// <summary>Stops any currently running DoT.</summary>
    public void StopDamageOverTime()
    {
        if (dotRoutine != null)
        {
            StopCoroutine(dotRoutine);
            dotRoutine = null;
        }
    }

    private IEnumerator DamageOverTime(float dps, float duration, float tick)
    {
        float elapsed = 0f;
        while (elapsed < duration && !IsDead)
        {
            if (!invulnerable)
            {
                float dt = Mathf.Min(tick, duration - elapsed);
                TakeDamage(dps * dt);
            }
            elapsed += tick;
            yield return new WaitForSeconds(tick);
        }
        dotRoutine = null;
    }

    // Handy context menu utilities in the Inspector
    [ContextMenu("Kill")] public void Kill() => TakeDamage(Mathf.Infinity);
    [ContextMenu("Full Heal")] public void FullHeal() => SetHealth(maxHealth);
}
