using Unity.VisualScripting;
using UnityEngine;

public class InvertCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Flip normals
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        mesh.normals = normals;

        // Flip triangle winding order
        for (int subMesh = 0; subMesh < mesh.subMeshCount; subMesh++)
        {
            int[] tris = mesh.GetTriangles(subMesh);
            for (int i = 0; i < tris.Length; i += 3)
            {
                // swap winding order
                int temp = tris[i];
                tris[i] = tris[i + 1];
                tris[i + 1] = temp;
            }
            mesh.SetTriangles(tris, subMesh);
        } 
        
        MeshCollider meshCollider = this.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        
    }
    
}
