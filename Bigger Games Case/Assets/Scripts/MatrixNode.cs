using UnityEngine;

public class MatrixNode : MonoBehaviour, IResetable
{
    [SerializeField] private MatrixNodeData data;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color passiveColor;

    public void OnEnable()
    {
        ((IResetable)this).Subscription();
        GridTable.clearGridHighlight += ResetGridColor;
    }
    
    public void OnDisable()
    {
        ((IResetable)this).Unsubscription();
        GridTable.clearGridHighlight -= ResetGridColor;
    }

    public void Init(MatrixNodeData matrixNodeData)
    {
        data = matrixNodeData;
        transform.position = new Vector3(data.coordinate.x, data.coordinate.y, 0);
        gameObject.name = $"MatrixNode[{data.coordinate.x},{data.coordinate.y}]";
    }

    public void SetNode(Node node)
    {
        data = new MatrixNodeData(data.coordinate, true, node);
    }

    public void UnsetNode()
    {
        data = new MatrixNodeData(data.coordinate, false, null);
    }

    public bool HasNode()
    {
        return data.hasNode;
    }
    public Vector2Int GetCoordinate()
    {
        return data.coordinate;
    }


    void IResetable.Reset()
    {
        UnsetNode();
    }

    private void ResetGridColor()
    {
        SetHighlight(false);
    }
    public void SetHighlight(bool state)
    {
        if (state)
        {
            spriteRenderer.color = activeColor;
        }
        else
        {
            spriteRenderer.color = passiveColor;
        }
    }
}