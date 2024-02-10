using UnityEngine;
using Unity.Mathematics;

public class PuzzleTable : MonoBehaviour
{
    [SerializeField] private GameObject gridCube;
    [SerializeField] private PuzzleGenerator puzzleGenerator;

    private GameObject[,] _grid;
    

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GameObject GridHolder = new GameObject("GridHolder");
        int size = puzzleGenerator.puzzleSize;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var a = Instantiate(gridCube, new Vector3(i, j, 0), quaternion.identity);
                a.transform.parent = GridHolder.transform;
            }
        }
    }

   
}