using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using Color = UnityEngine.Color;

public class Piece : PuzzleItem
{
    [SerializeField] protected LeanDragTranslate leanDragTranslate;
    [SerializeField] protected LeanSelectableByFinger leanSelectableByFinger;
    [SerializeField] private List<Node> _nodes = new();
    [SerializeField] private GameObject orginPoint;
    [SerializeField] private GridTable table;

    private readonly WaitForEndOfFrame _endOfFrame = new();
    private Vector3 lastDragPosition;
    private Node _firstNode;
    private int _nodeCount;
    private bool _isPlaced, _inMatrixNode, _isFingerDown;
    private const float SnapDistance = 1f;
    public static Action<bool, Piece> onPieceStateChanged;

    private void OnEnable()
    {
        LevelManager.onLevelReload += ReloadPuzzle;
    }

    private void OnDisable()
    {
        LevelManager.onLevelReload -= ReloadPuzzle;
    }

    public void Init(Transform parent, Color groupColor)
    {
        transform.parent = parent;
        gameObject.name = $"Piece_{groupColor.r}.{groupColor.g}.{groupColor.b}";
        leanSelectableByFinger.OnSelectedFingerUp.AddListener(OnFingerUp);
        leanSelectableByFinger.OnSelectedFinger.AddListener(OnFingerDown);
    }

    private void Start()
    {
        table = PuzzleGenerator.Instance.GridTable;
    }

    private void OnFingerUp(LeanFinger leanFinger)
    {
        HapticFeedBack();
        TryShifting();
        ChangeLayerOnFingerUp();
        _isFingerDown = false;
        StopCoroutine(HoldCoroutine());
    }
    
    private void OnFingerDown(LeanFinger leanFinger)
    {
        SoundManager.Instance.PlaySound("Click");
        HapticFeedBack();
        UnRegisterNodes();
        ChangeLayerOnFingerDown();
        _isFingerDown = true;
        StartCoroutine(HoldCoroutine());
    }

    private void UnRegisterNodes()
    {
        if (_isPlaced)
        {
            foreach (var node in _nodes)
            {
                node.UnRegisterMatrixNode();
            }
            onPieceStateChanged?.Invoke(false, this);
        }
        _isPlaced = false;
    }

    private IEnumerator HoldCoroutine()
    {
        while (_isFingerDown)
        {
            GridTable.clearGridHighlight?.Invoke();
            MatrixNodeCheck();
            yield return _endOfFrame;
        }
    }

    private void MatrixNodeCheck()
    {
        if (TryGetPair(out var pair))
        {
            return;
        }
        table.TryHighlight(_nodes, _firstNode, pair.Item2);
    }

    private void TryShifting()
    {
        GridTable.clearGridHighlight?.Invoke();
        if (TryGetPair(out var pair))
        {
            PlaceBack();
            return;
        }
        var t = table.TryAssignNodes(_nodes, _firstNode, pair.Item2);
        if (!t.Item1)
        {
            PlaceBack();
            return;
        }

        if (!_isPlaced)
        {
            onPieceStateChanged?.Invoke(true, this);
        }

        _isPlaced = true;
        Shift(t.Item2);
    }

    private void PlaceBack()
    {
        SoundManager.Instance.PlaySound("NotPlaced");
        transform.position = lastDragPosition;
    }

    private bool TryGetPair(out (bool, MatrixNode) pair)
    {
        var nodePos = _firstNode.transform.position;
        pair = table.GetClosestEmptyMatrixNode(nodePos);

        if (!pair.Item1)
        {
            return true;
        }

        float distance = Vector3.Distance(nodePos, pair.Item2.transform.position);
        if (distance > SnapDistance)
        {
            _inMatrixNode = false;
            return true;
        }

        return false;
    }
    private void HapticFeedBack()
    {
        if (!Settings.Instance.vibrationToggle.isOn)
        {
            return;
        }

        CandyCoded.HapticFeedback.HapticFeedback.LightFeedback();
    }

    private void ReloadPuzzle()
    {
        if (GridTable.OnAnimation)
        {
            return;
        }
        transform.position = lastDragPosition;
        UnRegisterNodes();
    }

    public void StartPosition()
    {
        lastDragPosition = transform.position;
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

    public Vector2Int ReturnPieceSize()
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (var node in _nodes)
        {
            if (node.Coordinate.x < minX)
            {
                minX = node.Coordinate.x;
            }

            if (node.Coordinate.y < minY)
            {
                minY = node.Coordinate.y;
            }

            if (node.Coordinate.x > maxX)
            {
                maxX = node.Coordinate.x;
            }

            if (node.Coordinate.y > maxY)
            {
                maxY = node.Coordinate.y;
            }
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        Debug.Log($"Piece Size width:{width}, height:{height}");
        return new Vector2Int(width, height);
    }


    public void Shift(Vector3 value)
    {
        SoundManager.Instance.PlaySound("Placed");
        transform.position += value;
    }


    public bool HasSingleNode()
    {
        return _nodeCount == 1;
    }

    private void ChangeLayerOnFingerUp()
    {
        foreach (var node in _nodes)
        {
            node.SortingLayerDown();
        }
    }

    private void ChangeLayerOnFingerDown()
    {
        transform.position += new Vector3(0, 1.5f, 0);

        foreach (var node in _nodes)
        {
            node.SortingLayerUp();
        }
    }

    public void RemoveAllNodes()
    {
        foreach (var node in _nodes)
        {
            node.RemoveFromNode();
        }

        Destroy(gameObject);
    }

    public void ShiftNodesToOrigin()
    {
        Vector3 offset = Vector3.zero;
        foreach (var node in _nodes)
        {
            offset += node.transform.localPosition;
        }

        offset /= _nodeCount;
        foreach (var node in _nodes)
        {
            node.transform.localPosition -= offset;
        }
    }
}