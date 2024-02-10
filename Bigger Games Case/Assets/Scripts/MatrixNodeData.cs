using System;
using UnityEngine;

[Serializable]
public class MatrixNodeData
{
    public Vector2Int coordinate;
    public bool hasNode;
    public Node node;

    public MatrixNodeData(Vector2Int coordinate, bool hasNode, Node node)
    {
        this.coordinate = coordinate;
        this.hasNode = hasNode;
        this.node = node;
    }
}