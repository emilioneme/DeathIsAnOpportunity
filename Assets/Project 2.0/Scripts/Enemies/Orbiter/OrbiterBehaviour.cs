using UnityEngine;

public class OrbiterBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsDead) gameObject.SetActive(false);
        if (gameObject.HasLineOfSight( target, 50)) Orbit();
        else if (gameObject.HasLineOfSight(target, 10000)) Chase();
        else if (transform.position.y < 30) Fly();
    }

    private void Orbit()
    {
        transform.RotateAround(target.transform.position, Vector3.up, 5f * Time.deltaTime);

        // Optional: look at the target
        transform.LookAt(target.transform.position);
    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 5*Time.deltaTime);
    }

    private void Fly()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position+new Vector3(0,100f,0), 5*Time.deltaTime);
    }
}
