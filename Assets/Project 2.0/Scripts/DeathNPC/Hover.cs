using System;
using Unity.Mathematics;
using UnityEngine;
public class Hover : MonoBehaviour
{
    private float curTime = 0;
    [SerializeField] private float yPos;
    [SerializeField] private float hoverDistance;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float noiseSpeed;

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        Vector3 pos = gameObject.transform.position;
        pos.y = yPos + Mathf.Sin(curTime*hoverSpeed) * hoverDistance 
                                                     * Mathf.PerlinNoise(curTime*noiseSpeed, curTime*noiseSpeed);
        gameObject.transform.position = pos;

    }
}
