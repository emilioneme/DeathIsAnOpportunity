using UnityEngine;

public class HoverHand : MonoBehaviour
{
    [SerializeField] private float hoverDistance = 0.5f;  // max vertical offset
    [SerializeField] private float hoverSpeed = 2f;       // speed of hover
    [SerializeField] private float noiseSpeed = 1f;       // speed of Perlin noise
    [SerializeField] private float noiseAmount = 0.2f;    // amplitude of noise

    private float curTime = 0f;

    void Update()
    {
        curTime += Time.deltaTime;

        // Hover offset using Sin
        float hoverOffset = Mathf.Sin(curTime * hoverSpeed) * hoverDistance;

        // Optional subtle noise
        float noiseOffset = (Mathf.PerlinNoise(curTime * noiseSpeed, 0f) - 0.5f) * 2f * noiseAmount;

        // Apply hover relative to current position
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            hoverOffset + noiseOffset-0.3f,
            transform.localPosition.z
        );
    }
}
