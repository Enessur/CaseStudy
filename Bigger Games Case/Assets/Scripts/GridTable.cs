using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridTable : MonoBehaviour, IResetable
{
    [SerializeField] private MatrixNode matrixNodePrefab;
    private Dictionary<Vector2Int, MatrixNode> _matrixNodeDictionary = new();
    private int _occupiedNodeCount;
    private int _targetCount;
    private Vector2Int _size;

    public static Action onGridCreate;

    public void OnEnable()
    {
        ((IResetable)this).Subscription();
    }
    
    public void OnDisable()
    {
        ((IResetable)this).Unsubscription();
    }

    public void GenerateGrid(Vector2Int size)
    {
        GameObject gridHolder = new GameObject("GridHolder");
        _size = size;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var mn = Instantiate(matrixNodePrefab, gridHolder.transform, true);
                mn.Init(new MatrixNodeData(new Vector2Int(i, j), false, null));
                _matrixNodeDictionary.Add(new Vector2Int(i,j),mn);
            }
        }
        _targetCount = size.x * size.y;
        onGridCreate?.Invoke();
    }

    public void RemoveGrids()
    {
        foreach (var matrixNode in _matrixNodeDictionary.Values)
        {
            Destroy(matrixNode.gameObject);
        }
        _matrixNodeDictionary.Clear();
        GameObject gridHolder = GameObject.Find("GridHolder");
        if (gridHolder != null)
        {
            Destroy(gridHolder);
        }
    }
    


    public (bool, MatrixNode) GetClosestEmptyMatrixNode(Vector3 position)
    {
        if (!_matrixNodeDictionary.Any())
        {
            return (false, null);
        }

        float closest = float.MaxValue;
        MatrixNode closestNode = null;
        foreach (var k in _matrixNodeDictionary)
        {
            if (k.Value.HasNode())
            {
                continue;
            }

            var distance = Vector3.Distance(k.Value.transform.position, position);
            if (distance > closest)
            {
                continue;
            }

            closest = distance;
            closestNode = k.Value;
        }

        return (true, closestNode);
    }
    


    void IResetable.Reset()
    {
        foreach (var k in _matrixNodeDictionary)
        {
            ((IResetable)k.Value).Reset();
        }

    }

    public (bool, Vector3) TryAssignNodes(List<Node> nodes,Node firsNode, MatrixNode matrixNode)
    {
        Vector2Int offset = matrixNode.GetCoordinate();
        List<MatrixNode> availableNodes = new();
        foreach (var node in nodes)
        {
            var c = node.Coordinate + offset;
            if (!(c.x < _size.x) || !(c.y < _size.y) || !(c.x >= 0) || !(c.y >= 0))
            {
                return (false, Vector3.zero);
            }

            var mn = _matrixNodeDictionary[new Vector2Int(c.x, c.y)];
           
            if (mn.HasNode())
            {
                return (false, Vector3.zero);
            }
            availableNodes.Add(mn);
        }
        
        
        int i = 0;
        foreach (var node in nodes)
        {
            node.RegisterMatrixNode(availableNodes[i]);
            availableNodes[i].SetNode(node);
            i++;
        }
        return (true, matrixNode.transform.position - firsNode.transform.position);
    }
}