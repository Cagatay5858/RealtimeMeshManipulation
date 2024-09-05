using UnityEngine;

public class MeshSaver : MonoBehaviour
{
    public MeshFilter meshFilter; // MeshFilter komponentini inspector'dan ba�lay�n
    public string filePath = "C:\\OUAUnity\\RPGgame\\Assets\\mesh.obj"; // Dosya yolunu uygun �ekilde de�i�tirin

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            MeshExporter exporter = new MeshExporter();
            exporter.ExportMeshToObj(meshFilter, filePath);
            Debug.Log("Mesh exported to: " + filePath);
        }
    }
}
