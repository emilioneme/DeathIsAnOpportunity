using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class ChaseBehaviour : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] BreadcrumbManager breadcrumbs;
    [SerializeField] Health health;
    
    
    private Vector3? currentBreadcrumb;
    private int lastSeenIndex = 0;
    private int currentIndex = -1;

    public Vector3 proximityOffsetDir = Vector3.zero;
    private EnemySpawnInfo spawnInfo;
    private EnemyAttack attack;


    void Awake()
    {
        spawnInfo = GetComponent<EnemySpawnInfo>();
        attack = GetComponent<EnemyAttack>();
        
    }

    IEnumerator Setup()
    {
        // Wait until targetRef is assigned (or just one frame)
        yield return new WaitUntil(() => spawnInfo.targetRef != null);

        target = spawnInfo.targetRef;
        attack.target = spawnInfo.targetRef;
        breadcrumbs = spawnInfo.targetRef.GetComponent<BreadcrumbManager>();

        transform.LookAt(target.transform);

        breadcrumbs.onBreadcrumbsDelete += ()=>lastSeenIndex--;
        breadcrumbs.onBreadcrumbsDelete += ()=>currentIndex--;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Setup());
    
    }

    void Update()
    {
        
        if(health.IsDead) gameObject.SetActive(false);
        if (target == null) return;
        // track breadcrumb index while LOS
        if (gameObject.HasLineOfSight( target, 6000)) Chase();
        else if (currentIndex < Mathf.Min(lastSeenIndex+10,breadcrumbs.lastIndex()) && currentIndex > 0 && lastSeenIndex > 0)
        {
            // Debug.Log($"currentIdx: {currentIndex}, lastSeenIdx: {lastSeenIndex}");
            if (Vector3.Magnitude((Vector3)(currentBreadcrumb - transform.position)) < 0.1)
                currentBreadcrumb = breadcrumbs.GetBreadcrumbAt(currentIndex++);

            if(currentBreadcrumb.HasValue) FollowBreadCrumbs(currentBreadcrumb.Value);
        }
        else
        {
            // Debug.Log("Patrol");
        }
    }

    private void FollowBreadCrumbs(Vector3 target)
    {
        // transform.LookAt(Vector3.Lerp(transform.forward, target, 0.5f));
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime*5f); 
    
    }

    void Chase()
    {
        if (target == null) return;
        lastSeenIndex = breadcrumbs.lastIndex();
        
        // might be expencive
        currentIndex = Mathf.Max(0, lastSeenIndex - 10);
        // currentIndex = breadcrumbs.closestBreadcrumbIndex(gameObject.transform.position);
        currentBreadcrumb = breadcrumbs.GetBreadcrumbAt(currentIndex);
        
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position+proximityOffsetDir, Time.deltaTime*(2f
            + (target.transform.position - transform.position).magnitude/5) ); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ChaseBehaviour>())
        {
            Debug.Log("ChaseR");
        }
    }
}
