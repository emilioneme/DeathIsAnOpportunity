using Unity.VisualScripting;
using UnityEngine;

public class ChaseBehaviour : MonoBehaviour
{
    [SerializeField] GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.LookAt(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime*(2f
           + (target.transform.position - transform.position).magnitude/5)
            
            
            
            );
    }
}
