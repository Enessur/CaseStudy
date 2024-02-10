using System;
using System.Collections.Generic;
using UnityEngine;

public class GridTable : MonoBehaviour
{
    [SerializeField] private MatrixNode matrixNodePrefab;
    public Action onAllNodesOccupied;
    private Dictionary<Vector2Int, MatrixNode> _matrixNodeDictionary = new();
    private int _occupiedNodeCount;
    private int _targetCount;


    public void GenerateGrid(Vector2Int size)
    {
        GameObject gridHolder = new GameObject("GridHolder");

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var mn = Instantiate(matrixNodePrefab, gridHolder.transform, true);
                mn.Init(new MatrixNodeData(new Vector2Int(i, j), false, null));
                mn.onNodeOccupied += OnNodeOccupied;
            }
        }

        _targetCount = size.x * size.y;
    }

    private void OnNodeOccupied()
    {
        _occupiedNodeCount++;
        CheckComplete();
    }

    private void CheckComplete()
    {
        if (_occupiedNodeCount < _targetCount)
        {
            return;
        }

        onAllNodesOccupied?.Invoke();
    }

    private void Reset()
    {
        foreach (var k in _matrixNodeDictionary)
        {
            k.Value.Reset();
        }

        _occupiedNodeCount = 0;
    }
}