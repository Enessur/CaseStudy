using UnityEngine;

public class ShapeFiller : MonoBehaviour
{
    void Start()
    {
        GenerateTangram();
    }

    void GenerateTangram()
    {
        // Kare içindeki noktaları belirle
        Vector3[] squarePoints = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 4, 0),
            new Vector3(4, 4, 0),
            new Vector3(4, 0, 0)
        };

        // Prosedürel olarak parçaları oluştur ve çiz
        for (int i = 0; i < 6; i++)
        {
            Vector3[] piecePoints = GenerateSpecificPiece(squarePoints, i);
            DrawPiece(piecePoints, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
        }
    }

    void DrawSquare(Vector3[] points)
    {
        Mesh squareMesh = new Mesh();
        squareMesh.vertices = points;

        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        squareMesh.triangles = triangles;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = squareMesh;
    }

   Vector3[] GenerateSpecificPiece(Vector3[] squarePoints, int pieceIndex)
    {
        // Rastgele parça noktalarını oluştur
        Vector3[] piecePoints = new Vector3[9];
        switch (pieceIndex)
        {
            case 0: // Kare
                piecePoints = new Vector3[]
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(0 , 2),
                    new Vector2(1 , 2),
                    new Vector2(2 , 2),
                    new Vector2(2 , 1),
                    new Vector2(2 , 0),
                    new Vector2(1 , 0),
                    new Vector2(0 , 0),
         
                };
                break;
            
            default:
                break;
        }

        return piecePoints;
    }

    void DrawPiece(Vector3[] points, Color color)
    {
        Mesh pieceMesh = new Mesh();
        pieceMesh.vertices = points;

        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7, 0, 7, 8 };
        pieceMesh.triangles = triangles;

        GameObject pieceObject = new GameObject("TangramPiece");
        MeshRenderer meshRenderer = pieceObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.material.color = color;

        MeshFilter meshFilter = pieceObject.AddComponent<MeshFilter>();
        meshFilter.mesh = pieceMesh;

        Rigidbody rigidbody = pieceObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false; // Parçalar aşağıya düşmesin

        BoxCollider boxCollider = pieceObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
}
