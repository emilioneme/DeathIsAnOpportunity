using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChaserManager : MonoBehaviour
{
    [SerializeField] List<ChaseBehaviour> chasers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < chasers.Count; i++)
        {
            chasers[i].proximityOffsetDir = Vector3.zero;
            for (int j = 0; i < chasers.Count; i++)
            {
                if (i != j)
                {
                    if (Vector3.Distance(chasers[i].transform.position, chasers[j].transform.position) < 20f)
                    {
                        chasers[i].proximityOffsetDir += (chasers[i].transform.position - chasers[j].transform.position).normalized*0.02f;
                        chasers[i].proximityOffsetDir.y = 0;
                    }
                }
            }
        }
    }
    
    
}
