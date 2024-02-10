using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    public int gridSize = 4;
    private List<Mesh> shapes = new List<Mesh>();

    void Start()
    {
        GenerateShapes();
        PlaceShapes();
    }

    void GenerateShapes()
    {
        // Örnek olarak 6 farklı şekil oluşturalım
        for (int i = 0; i < 6; i++)
        {
            shapes.Add(CreateRandomShape());
        }
    }

    Mesh CreateRandomShape()
    {
        Mesh mesh = new Mesh();

        // Rastgele vertex'ler oluştur
        List<Vector3> vertices = new List<Vector3>();
        int vertexCount = Random.Range(4, 10); // Rastgele 4 ile 10 arası vertex sayısı

        for (int i = 0; i < vertexCount; i++)
        {
            vertices.Add(new Vector3(Random.Range(0, gridSize), Random.Range(0, gridSize), 0));
        }

        // Triangulation algoritması ile üçgenleri belirleyin
        // Bu örnek kodda, üçgenler rastgele veya basit bir düzen kullanılarak oluşturulmaz.
        // Gerçek bir uygulamada, bir triangulation kütüphanesi kullanmanız gerekir.

        mesh.vertices = vertices.ToArray();

        // Generate triangles using the vertices
        int[] triangles = new int[(vertexCount - 2) * 3];
        int index = 0;
        for (int i = 1; i < vertexCount - 1; i++)
        {
            triangles[index++] = 0;
            triangles[index++] = i;
            triangles[index++] = i + 1;
        }

        mesh.triangles = triangles;

        // Normalleri manuel olarak ayarlama (Opsiyonel, şu satırı kaldırarak dene)
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();  // Mesh sınırlarının hesaplanması

        return mesh;
    }

    void PlaceShapes()
    {
        foreach (var mesh in shapes)
        {
            // Yeni bir GameObject oluştur ve MeshFilter ve MeshRenderer bileşenlerini ekle
            GameObject shapeObject = new GameObject("RandomShape");
            var meshFilter = shapeObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        
            var meshRenderer = shapeObject.AddComponent<MeshRenderer>();
        
            // Create a simple material (you can adjust the material settings in the Unity Editor)
            Material material = new Material(Shader.Find("Standard"));
        
            // Assign the material to the MeshRenderer
            meshRenderer.material = material;

            // Rigidbody2D bileşeni ekle
            var rigidbody2D = shapeObject.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0; // Yerçekimini kapat

            // GameObject'in transformunu düzenleyin (isteğe bağlı olarak konumlandırma vs.)
            // Örneğin, rastgele bir konum ve rotasyon atayabilirsiniz.
            shapeObject.transform.position = new Vector3(Random.Range(0, gridSize), Random.Range(0, gridSize), 0);
            shapeObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        }
    }
}
