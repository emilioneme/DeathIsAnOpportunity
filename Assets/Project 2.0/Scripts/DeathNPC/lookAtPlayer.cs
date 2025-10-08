using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    
    [SerializeField] GameObject player;
    [SerializeField] private Transform bodyTransform;
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Angle(bodyTransform.forward, player.transform.position - transform.position) < 60)
        gameObject.transform.LookAt(player.transform.position);
    }
}
