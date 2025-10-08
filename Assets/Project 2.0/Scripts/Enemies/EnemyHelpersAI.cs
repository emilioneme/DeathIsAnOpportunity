using UnityEngine;
public static class GameObjectExtensions{
    public static bool HasLineOfSight(this GameObject emitter, GameObject target, int sightRange)
    {
        int mask = LayerMask.GetMask("Player", "Default");
        
        Vector3 directionToPlayer = (target.transform.position - emitter.transform.position).normalized;
        float distanceToTarget = Vector3.Distance(emitter.transform.position, target.transform.position);

        // Raycast towards the target
        if (Physics.Raycast(emitter.transform.position, directionToPlayer, out RaycastHit hit, sightRange, mask))
        {
            // TODO: maybe better way?
            if (hit.transform.name == target.name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

}
