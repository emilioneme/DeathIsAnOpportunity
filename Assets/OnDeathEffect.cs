using UnityEngine;

public class OnDeathEffect : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject damageEffect;
    void Awake()
    {
         GetComponent<Health>().OnDeath.AddListener((() => {Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 2);}));       
         GetComponent<Health>().OnDamaged.AddListener((() => {Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 2);}));   
    }
}
