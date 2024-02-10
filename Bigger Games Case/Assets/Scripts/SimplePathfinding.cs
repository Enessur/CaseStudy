using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class SimplePathfinding : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab olarak kullanılacak obje (örneğin, Cube)
    private GameObject[,] grid = new GameObject[4, 4]; // 4x4 grid
    private Color[] shapeColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan }; // 6 farklı renk
    private bool[,] visited = new bool[4, 4]; // Ziyaret edilmiş kareleri takip etmek için

    void Start()
    {
        InitializeGrid();
        CreateAndColorGroups();
    }

    void InitializeGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject cube = Instantiate(cubePrefab, new Vector3(i, j, 0), Quaternion.identity);
                cube.GetComponent<Renderer>().material.color = Color.white; // Başlangıçta tüm küpleri beyaz renkte ayarla
                grid[i, j] = cube;
            }
        }
    }

    void CreateAndColorGroups()
    {
        for (int groupIndex = 0; groupIndex < 5; groupIndex++)
        {
            // Rastgele iki farklı başlangıç noktası seç
            Vector2Int start = GetRandomPoint();
            Vector2Int end = GetRandomPoint(start);

            // Yol bulma
            List<Vector2Int> path = FindPath(start, end);

            // Grup oluşturma ve renklendirme
            CreateAndColorGroup(path, shapeColors[groupIndex]);
        }
    }

    Vector2Int GetRandomPoint(Vector2Int? invalidPoint = null)
    {
        Vector2Int randomPoint;

        do
        {
            randomPoint = new Vector2Int(Random.Range(0, 4), Random.Range(0, 4));
        } while (invalidPoint.HasValue && randomPoint == invalidPoint.Value);

        return randomPoint;
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        // NavMeshAgent oluştur
        GameObject agentObject = new GameObject("Agent");
        NavMeshAgent agent = agentObject.AddComponent<NavMeshAgent>();

        // Agent'ın başlangıç pozisyonunu ve hedefini belirle
        agent.transform.position = new Vector3(start.x, 0, start.y);
        agent.SetDestination(new Vector3(end.x, 0, end.y));

        // Yol noktalarını al
        List<Vector2Int> resultPath = new List<Vector2Int>();
        foreach (Vector3 point in agent.path.corners)
        {
            resultPath.Add(new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.z)));
        }

        // Agent objesini temizle
        Destroy(agentObject);

        return resultPath;
    }

    void CreateAndColorGroup(List<Vector2Int> path, Color color)
    {
        GameObject groupObject = new GameObject("Group");
        groupObject.transform.parent = transform; // Ana objeyi GridManager'ın child'i yap

        foreach (Vector2Int point in path)
        {
            // Aynı renkteki kareleri grup objesinin child'ı yap ve renklendir
            grid[point.x, point.y].transform.parent = groupObject.transform;
            grid[point.x, point.y].GetComponent<Renderer>().material.color = color;

            visited[point.x, point.y] = true;
        }
    }
}