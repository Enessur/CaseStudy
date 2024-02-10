using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cubePrefab;  
    private GameObject[,] grid = new GameObject[4, 4];  

    void Start()
    {
        InitializeGrid();
        CreateAndGroupCubes();
    }

    void InitializeGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject cube = Instantiate(cubePrefab, new Vector3(i, j, 0), Quaternion.identity);
                cube.GetComponent<Renderer>().material.color =
                    Color.white;  
                grid[i, j] = cube;
            }
        }
    }

    void CreateAndGroupCubes()
    {
        
        int startX = Random.Range(0, 4);
        int startY = Random.Range(0, 2); 

        
        GroupAndColor(startX, startY, 3, Color.blue);
    }

    void GroupAndColor(int startX, int startY, int groupSize, Color color)
    {
        GameObject groupObject = new GameObject("Group");
        groupObject.transform.parent = transform;  

        for (int i = 0; i < groupSize; i++)
        {
            int x = startX;
            int y = startY;
            int r = Random.Range(0, 2);
            if (r == 1)
            {
                x = startX + i;
            }

            if (r == 0)
            {
                y = startY + i;
            }

            if (x < 0 || x >= 4 || y < 0 || y >= 4)
            {
                Debug.LogError("Invalid group position.");
                return;
            }
            grid[x, y].transform.parent = groupObject.transform;
            grid[x, y].GetComponent<Renderer>().material.color = color;
        }
    }
}