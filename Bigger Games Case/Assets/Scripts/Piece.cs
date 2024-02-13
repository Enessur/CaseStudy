using System;
using System.Collections.Generic;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : PuzzleItem
{
    [SerializeField] protected LeanDragTranslate leanDragTranslate;
    [SerializeField] protected LeanSelectableByFinger leanSelectableByFinger;
    [SerializeField] private List<Node> _nodes = new();

    private Node _firstNode;
    private int _nodeCount;
    private bool _isPlaced;
    private const float SnapDistance = 1f;
    public static Action<bool,Piece> onPieceStateChanged;
    public void Init(Transform parent, Color groupColor)
    {
        transform.parent = parent;
        gameObject.name = $"Piece_{groupColor.r}.{groupColor.g}.{groupColor.b}";
        leanSelectableByFinger.OnSelectedFingerUp.AddListener(OnFingerUp);
        leanSelectableByFinger.OnSelectedFinger.AddListener(OnFingerDown);
    }

    private void OnFingerUp(LeanFinger leanFinger)
    {
        TryShifting();
    }

    private void OnFingerDown(LeanFinger leanFinger)
    {
        UnRegisterNodes();
    }

    private void UnRegisterNodes()
    {
        if (_isPlaced)
        {
            foreach (var node in _nodes)
            {
                node.UnRegisterMatrixNode();
            }
            onPieceStateChanged?.Invoke(false,this);
        }
        
        _isPlaced = false;
    }


    private void TryShifting()
    {
        GridTable table = PuzzleGenerator.Instance.GridTable;
        var nodePos = _firstNode.transform.position;
        var pair = table.GetClosestEmptyMatrixNode(nodePos);

        if (!pair.Item1)
        {
            Debug.Log("Not Found!!");
            return;
        }

        if (Vector3.Distance(nodePos, pair.Item2.transform.position) > SnapDistance)
        {
            return;
        }

        var t = table.TryAssignNodes(_nodes, _firstNode, pair.Item2);
        if (!t.Item1)
        {
            Debug.Log("Cant Assign");
            return;
        }

        if (!_isPlaced)
        {
            onPieceStateChanged?.Invoke(true,this);
        }
        _isPlaced = true;
        Shift(t.Item2);
        
    }

    public void AddNode(Node node)
    {
        _nodes.Add(node);
        if (_nodeCount == 0)
        {
            _firstNode = node;
        }

        _nodeCount++;
    }

    public void RecalculateCoordinates()
    {
        var position = _firstNode.transform.position;
        Vector2Int offset = new Vector2Int((int)position.x, (int)position.y);
        foreach (Node node in _nodes)
        {
            node.RemoveOffset(offset);
        }
    }


    public void SetNodesColor(Color newColor)
    {
        foreach (Node node in _nodes)
        {
            node.SetColor(newColor);
        }
    }


    public void Shift(Vector3 value)
    {
        transform.position += value;
    }


    public bool HasSingleNode()
    {
        return _nodeCount == 1;
    }

    public void RemoveAllNodes()
    {
        foreach (var node in _nodes)
        {
            node.RemoveFromNode();
        }
        
        Destroy(gameObject);
    }
}