using UnityEngine;

public class TangramPieceGenerator : MonoBehaviour
{
    public GameObject tangramPiecePrefab; // Tangram parça prefab'ı
    public int gridSize = 4; // Kare ızgaranın boyutu

    void Start()
    {
        GenerateTangramPuzzle();
    }

    void GenerateTangramPuzzle()
    {
        float cellSize = 1f; // Kare hücre boyutu

        // Tangram parça türleri ve kenar uzunlukları
        string[] pieceTypes = { "BigSquare", "MediumSquare", "SmallSquare", "BigTriangle", "SmallTriangle" };
        float[] sideLengths = { 2f, 1.5f, 1f, 2f, 1f };

        // Başlangıç pozisyonu ve dönüş
        float startX = -cellSize * (gridSize / 2);
        float startY = cellSize * (gridSize / 2);

        for (int i = 0; i < 5; i++)
        {
            // Rastgele bir parça türü seç
            int randomIndex = Random.Range(0, pieceTypes.Length);
            string selectedType = pieceTypes[randomIndex];
            float sideLength = sideLengths[randomIndex];

            // Rastgele bir pozisyon ve dönüş belirle
            float randomX = startX + Random.Range(0, gridSize) * cellSize;
            float randomY = startY - Random.Range(0, gridSize) * cellSize;
            float randomRotation = Random.Range(0, 4) * 90f;

            // Tangram parçasını oluştur ve konumlandır
            GameObject tangramPiece = Instantiate(tangramPiecePrefab, new Vector3(randomX, randomY, 0f), Quaternion.Euler(0f, 0f, randomRotation));

            // Tangram parçasını ölçeklendir
            tangramPiece.transform.localScale = new Vector3(sideLength, sideLength, 1f);

            // Oluşturulan parçaya özel bir isim ver (isteğe bağlı)
            tangramPiece.name = selectedType + "Piece";
        }
    }
}