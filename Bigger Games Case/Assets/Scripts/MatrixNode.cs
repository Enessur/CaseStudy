using System;
using UnityEngine;

public class MatrixNode : MonoBehaviour
{
    [SerializeField] private MatrixNodeData data;
    public Action onNodeOccupied;

    public void Init(MatrixNodeData matrixNodeData)
    {
        data = matrixNodeData;
        transform.position = new Vector3(data.coordinate.x, data.coordinate.y, 0);
    }

    public void SetNode(Node node)
    {
        data = new MatrixNodeData(data.coordinate, true, node);
        onNodeOccupied?.Invoke();
    }

    private void UnsetNode()
    {
        data = new MatrixNodeData(data.coordinate, false, null);
    }

    public void Reset()
    {
        UnsetNode();
    }
}