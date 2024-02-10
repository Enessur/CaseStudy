using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    void Start()
    {
        // Üçgenin köşe noktalarını belirle
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0.5f, 1, 0),
            new Vector3(1, 0, 0)
        };

        // Üçgenin üç kenarını belirle
        int[] triangles = new int[] { 0, 1, 2 };

        // Mesh oluştur
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // GameObject oluştur
        GameObject triangle = new GameObject("Triangle");

        // MeshFilter ve MeshRenderer ekle
        MeshFilter meshFilter = triangle.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = triangle.AddComponent<MeshRenderer>();

        // Mesh'i ata
        meshFilter.mesh = mesh;

        // Materyali ayarla (istediğiniz renk veya materyali ekleyebilirsiniz)
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // Opsiyonel olarak, üçgenin rengini değiştirebilirsiniz
        // meshRenderer.material.color = Color.red;
    }
}
