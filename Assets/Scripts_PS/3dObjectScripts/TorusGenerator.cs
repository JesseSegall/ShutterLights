using UnityEngine;
using System.Collections.Generic;

public class TorusGenerator : MonoBehaviour
{
    [Header("Flat Ring Parameters")]
    public float innerRadius = 2f;
    public float outerRadius = 4f;
    public float height = 1f;
    public int sides = 6; // Number of flat sides

    public Material customMaterial;

    void Start()
    {
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        mr.material = customMaterial != null ? customMaterial : new Material(Shader.Find("Standard"));

        Mesh mesh = GenerateFlatRing();
        mf.mesh = mesh;

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
        collider.convex = true; // if it moves, set convex true
    }

   Mesh GenerateFlatRing()
{
    Mesh mesh = new Mesh();

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    float halfHeight = height / 2f;

    for (int i = 0; i < sides; i++)
    {
        float angle = i * Mathf.PI * 2f / sides;
        float nextAngle = (i + 1) * Mathf.PI * 2f / sides;

        Vector3 outerCurrentTop = new Vector3(Mathf.Cos(angle) * outerRadius, halfHeight, Mathf.Sin(angle) * outerRadius);
        Vector3 outerNextTop = new Vector3(Mathf.Cos(nextAngle) * outerRadius, halfHeight, Mathf.Sin(nextAngle) * outerRadius);
        Vector3 innerCurrentTop = new Vector3(Mathf.Cos(angle) * innerRadius, halfHeight, Mathf.Sin(angle) * innerRadius);
        Vector3 innerNextTop = new Vector3(Mathf.Cos(nextAngle) * innerRadius, halfHeight, Mathf.Sin(nextAngle) * innerRadius);

        Vector3 outerCurrentBottom = outerCurrentTop - Vector3.up * height;
        Vector3 outerNextBottom = outerNextTop - Vector3.up * height;
        Vector3 innerCurrentBottom = innerCurrentTop - Vector3.up * height;
        Vector3 innerNextBottom = innerNextTop - Vector3.up * height;

        int vertIndex = vertices.Count;

        vertices.AddRange(new Vector3[] {
            // Top face vertices
            outerCurrentTop, outerNextTop, innerNextTop, innerCurrentTop,
            // Bottom face vertices
            outerCurrentBottom, outerNextBottom, innerNextBottom, innerCurrentBottom
        });

        // Top face
        triangles.AddRange(new int[] {
            vertIndex, vertIndex + 1, vertIndex + 2,
            vertIndex, vertIndex + 2, vertIndex + 3
        });

        // Bottom face (flipped winding)
        triangles.AddRange(new int[] {
            vertIndex + 4, vertIndex + 6, vertIndex + 5,
            vertIndex + 4, vertIndex + 7, vertIndex + 6
        });

        // Outer side
        triangles.AddRange(new int[] {
            vertIndex, vertIndex + 5, vertIndex + 1,
            vertIndex, vertIndex + 4, vertIndex + 5
        });

        // Inner side
        triangles.AddRange(new int[] {
            vertIndex + 3, vertIndex + 2, vertIndex + 6,
            vertIndex + 3, vertIndex + 6, vertIndex + 7
        });
    }

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.RecalculateNormals(); // Ensure normals face outward
    mesh.RecalculateBounds();

    return mesh;
}
}