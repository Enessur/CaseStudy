using UnityEngine;

public class MatrixVisualizer : MonoBehaviour
{
    public GameObject ballPrefab; // Top prefab'ı

    void Start()
    {
        // 4x4 matrix tanımla
        Vector3[,] matrix = new Vector3[4, 4];

        // Matrix'i doldur ve görselleştir
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float x = i;
                float y = j;
                float z = 0;

                // Her bir vektör noktasına top oluştur
                GameObject ball = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
                ball.name = $"Vector({i}, {j})"; // Her topun adını belirle

                // Her bir vektörün değerini gösteren metin oluştur
                TextMesh textMesh = ball.AddComponent<TextMesh>();
                textMesh.text = $"({x}, {y}, {z})";
                textMesh.alignment = TextAlignment.Center;
                textMesh.anchor = TextAnchor.MiddleCenter;

                // Çizgileri çiz
                if (i < 3)
                    DrawLine(matrix[i, j], matrix[i + 1, j]);
                if (j < 3)
                    DrawLine(matrix[i, j], matrix[i, j + 1]);
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("Line");
        line.transform.position = (start + end) / 2;
        line.AddComponent<LineRenderer>().SetPositions(new Vector3[] { start, end });
    }
}