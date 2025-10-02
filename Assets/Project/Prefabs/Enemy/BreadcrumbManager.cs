using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadcrumbManager : MonoBehaviour
{
    private List<Vector3> breadcrumbs = new List<Vector3>();

    [SerializeField] private GameObject target;
    [SerializeField] private float breadcrumbInterval = 0.05f;
    [SerializeField] private int maxBreadcrumbs = 500;
    private int offset = 0;
    public event Action onBreadcrumbsDelete;
    
    void Start()
    {
       StartCoroutine(MakeBreadcrumbs()); 
    }

    IEnumerator MakeBreadcrumbs()
    {
        while (true)
        {
            breadcrumbs.Add(target.transform.position);
            if (breadcrumbs.Count > maxBreadcrumbs)
            {
                breadcrumbs.RemoveAt(0);
                onBreadcrumbsDelete?.Invoke();
            }
            
            yield return new WaitForSeconds(breadcrumbInterval);
        }
    }

    public int closestBreadcrumbIndex(Vector3 target)
    {
        int minIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < breadcrumbs.Count; i++)
        {
            float distance = Vector3.Distance(breadcrumbs[i], target);
            if (distance < minDistance)
            {
                minIndex = i;
                minDistance = distance;
            }
        }
        return minIndex;
    }
    public int lastIndex()
    {
        return breadcrumbs.Count - 1;
    }

    public Vector3? GetBreadcrumbAt(int index)
    {
       return  breadcrumbs[index];
    }
}
