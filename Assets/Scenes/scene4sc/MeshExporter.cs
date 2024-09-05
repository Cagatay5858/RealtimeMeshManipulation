using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MeshExporter : MonoBehaviour
{
    public void ExportMeshToObj(MeshFilter meshFilter, string filePath)
    {
        Mesh mesh = meshFilter.mesh;
        StreamWriter writer = new StreamWriter(filePath);

        // Write vertices
        foreach (Vector3 vertex in mesh.vertices)
        {
            writer.WriteLine($"v {vertex.x} {vertex.y} {vertex.z}");
        }

        // Write normals
        foreach (Vector3 normal in mesh.normals)
        {
            writer.WriteLine($"vn {normal.x} {normal.y} {normal.z}");
        }

        // Write UVs
        foreach (Vector2 uv in mesh.uv)
        {
            writer.WriteLine($"vt {uv.x} {uv.y}");
        }

        // Write faces
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int vertexIndex1 = mesh.triangles[i] + 1;
            int vertexIndex2 = mesh.triangles[i + 1] + 1;
            int vertexIndex3 = mesh.triangles[i + 2] + 1;
            writer.WriteLine($"f {vertexIndex1} {vertexIndex2} {vertexIndex3}");
        }

        writer.Close();
    }
}

