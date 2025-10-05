using UnityEngine;

public class OnDamageEffect : MonoBehaviour
{    [SerializeField] private GameObject deathEffect;
    void Awake()
    {
        GetComponent<Health>().OnDeath.AddListener((() => {Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 2);}));       GetComponent<Health>().OnDeath.AddListener((() => {Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 2);}));
    }
}
