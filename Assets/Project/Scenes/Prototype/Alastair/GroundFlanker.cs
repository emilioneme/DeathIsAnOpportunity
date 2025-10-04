using UnityEngine;
using UnityEngine.AI;

public class GroundFlanker_Behaviour : MonoBehaviour
{
    public Transform target;

    [Header("Spiral Settings")]
    public float spiralStrength = 5f; // Controls curve radius
    public float orbitSpeed = 5f;     // Controls how fast the spiral rotates
    public float minSpiralDistance = 2f;
    public float resumeSpiralDistance = 4f;

    private NavMeshAgent agent;
    private float angle; // No longer randomized
    private bool spiraling = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        angle = 0f; // Start angle at 0 for consistent behavior
    }

    void Update()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;

        // Toggle spiral mode based on distance
        if (distance < minSpiralDistance)
        {
            spiraling = false;
            angle = 0f; // Start angle at 0 for consistent behavior
        }
        else if (distance > resumeSpiralDistance)
        {
            spiraling = true;
        }

        Vector3 destination;

        if (spiraling)
        {
            // Calculate normalized direction to target
            Vector3 direction = toTarget.normalized;

            // Spiral around Y axis (terrain normal in most cases)
            Vector3 spiralDir = Vector3.Cross(direction, Vector3.up).normalized;

            // Increase angle over time
            angle += orbitSpeed * Time.deltaTime;

            // Use angle to generate circular offset
            float radians = angle * Mathf.Deg2Rad;
            Vector3 circularOffset = Mathf.Sin(radians) * spiralDir +
                                     Mathf.Cos(radians) * Vector3.Cross(spiralDir, Vector3.up);

            // Apply spiral strength as radius of circular offset
            Vector3 offset = circularOffset * spiralStrength;

            // Target a point offset from the direct path
            destination = target.position - direction * distance * 0.5f + offset;
        }
        else
        {
            destination = target.position;
        }

        agent.SetDestination(destination);
    }
}
