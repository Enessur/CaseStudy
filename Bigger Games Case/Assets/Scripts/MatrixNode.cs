using UnityEngine;

public class MatrixNode : MonoBehaviour
{
    [SerializeField] private MatrixNodeData data;

    public void Init(MatrixNodeData matrixNodeData)
    {
        data = matrixNodeData;
    }
    public void SetNode(Node node)
    {
        data = new MatrixNodeData(data.coordinate, true, node);
    }

    public void UnsetNode()
    {
        data = new MatrixNodeData(data.coordinate, false, null);
    }
}