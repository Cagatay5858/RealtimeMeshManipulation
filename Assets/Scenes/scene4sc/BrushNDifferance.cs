using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class BrushNDifferance : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;
    public float brushRadius = 1f; // Brush radius
    public float maxHeight = 0.5f; // Maximum height to raise

    private Vector3[] savedVertices; // Vertex konumlarýný kaydetmek için kullanýlacak dizi
    private bool verticesSaved = false; // Vertexlerin kaydedilip kaydedilmediðini kontrol eden bayrak
    private List<int> differentVertices = new List<int>(); // Farklý olan vertex indeksleri

    public Material lineMaterial; // Çizgiler için kullanýlacak malzeme

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ApplyBrush();
            UpdateMesh();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveVertices();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CompareVertices();
            DrawDifferences(); // Farklý vertex ve kenarlarý çiz
        }
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void ApplyBrush()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;

            for (int i = 0; i < vertices.Length; i++)
            {
                float distance = Vector3.Distance(hitPoint, transform.TransformPoint(vertices[i]));

                if (distance < brushRadius)
                {
                    float weight = 1 - (distance / brushRadius);
                    vertices[i].y += maxHeight * weight; // Vertex deformasyonu
                }
            }
            // Deformasyon sonrasý mesh güncellenir
            UpdateMesh();
        }
    }

    private void UpdateMesh()
    {
        // Mesh ve collider güncelleniyor
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();  // Normalleri yeniden hesapla, böylece ýþýklandýrma doðru görünür
        mesh.RecalculateBounds();   // Mesh'in sýnýrlarýný yeniden hesapla

        // Mesh collider'i daima mesh ile güncel tut
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void SaveVertices()
    {
        // Kaydedilen vertexler sadece kýyaslama için kullanýlýr
        savedVertices = new Vector3[vertices.Length];
        vertices.CopyTo(savedVertices, 0); // Mevcut vertex konumlarýný kaydet
        verticesSaved = true;
        Debug.Log("Vertices saved.");
    }

    private void CompareVertices()
    {
        if (!verticesSaved)
        {
            Debug.Log("No saved vertices to compare.");
            return;
        }

        differentVertices.Clear(); // Farklý vertexlerin listesini temizle

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] != savedVertices[i])
            {
                differentVertices.Add(i); // Farklý vertexleri listeye ekle
            }
        }
    }

    private void DrawDifferences()
    {
        foreach (int index in differentVertices)
        {
            Vector3 worldPosition = transform.TransformPoint(vertices[index]);

            // Küreyi çizmek yerine, oyun görünümünde görülebilen bir çizim kullan
            DrawSphere(worldPosition, 0.1f, Color.red);

            // Farklý vertexlere baðlý kenarlarý renklendirme
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (triangles[i] == index || triangles[i + 1] == index || triangles[i + 2] == index)
                {
                    Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
                    Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
                    Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

                    DrawLine(v0, v1, Color.red);
                    DrawLine(v1, v2, Color.red);
                    DrawLine(v2, v0, Color.red);
                }
            }
        }
    }

    private void DrawSphere(Vector3 position, float radius, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * radius;
        sphere.GetComponent<Renderer>().material.color = color;
        Destroy(sphere, 1f); // Küreyi 1 saniye sonra yok et
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        Destroy(line, 1f); // Çizgiyi 1 saniye sonra yok et
    }
}
