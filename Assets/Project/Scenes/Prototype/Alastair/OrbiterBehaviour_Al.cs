using UnityEngine;

public class OrbiterBehaviour_Al : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Health health;
    [Header("Target")]
    [SerializeField] private GameObject target;
    [Header("Line of Sight")]
    [SerializeField] private int orbitLOS = 25;
    [SerializeField] private int chaseLOS = 10000;
    [Header("Movement Height Limits")]
    [SerializeField] private int flyHeight = 30;
    [SerializeField] private int descentHeight = 10;
    [Header("Movement")]
    [SerializeField] private bool randomOrbit = false;
    [SerializeField] private float orbitSpeed = 5f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float flySpeed = 5f;

    private int direction = 1;
    private Vector3 orbitDirection = Vector3.up;
    private bool orbitLock = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsDead) gameObject.SetActive(false);
        if (gameObject.HasLineOfSight(target, orbitLOS))
        {
            Orbit();
            orbitLock = true;
        }
        else if (gameObject.HasLineOfSight(target, chaseLOS))
        {
            Chase();
            orbitLock = false;
        }
        // else if (transform.position.y < 30 && ascending == true) {

        // } 
        else
        {
            FlyUpAndDown();
            orbitLock = false;
        }
        ;
    }

    private void Orbit()
    {
        Vector3 heightOffset = heightVector();
        if (randomOrbit && !orbitLock)
        {
            int rand = Random.Range(0, 2);
            if (rand < 1)
            {
                orbitDirection = Vector3.down;
            }
            else
            {
                orbitDirection = Vector3.up;
            }
        }
        transform.RotateAround(target.transform.position, orbitDirection + heightOffset, orbitSpeed * Time.deltaTime);

        // Optional: look at the target
        transform.LookAt(target.transform.position);
    }

    private void Chase()
    {
        Vector3 heightOffset = heightVector();
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + heightOffset, chaseSpeed * Time.deltaTime);

        // Optional: look at the target
        transform.LookAt(target.transform.position);
    }

    // private void Fly()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 100f, 0), flySpeed * Time.deltaTime);

    //     // Optional: look at the target
    //     transform.LookAt(target.transform.position);
    // }

    private void FlyUpAndDown()
    {
        if (transform.position.y > flyHeight)
        {
            direction = -1;
        }
        else if (transform.position.y < descentHeight)
        {
            direction = 1;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 100f * direction, 0), flySpeed * Time.deltaTime);

        // Optional: look at the target
        transform.LookAt(target.transform.position);
    }

    private Vector3 heightVector()
    {
        if (transform.position.y > flyHeight)
        {
            return new Vector3(0, -100f, 0);
        }
        else if (transform.position.y < descentHeight)
        {
            return new Vector3(0, 100f, 0);
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }
}
