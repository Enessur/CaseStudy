using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Node : PuzzleItem
{
    [SerializeField] private Renderer renderer;
    public Color Color => _color;
    private Color _color;
    public Piece Piece => _piece;
    private Piece _piece;
    public bool HasPiece => _hasPiece;
    private bool _hasPiece; 
    public Vector2Int Coordinate =>_coordinate;
    
    public static Action onReload;
    
    [SerializeField]private Vector2Int _coordinate;
    [SerializeField]private MatrixNode _matrixNode;
    private bool _hasMatrixNode;

    public void Init()
    {
    }

    private void AssignName()
    {
        gameObject.name = $"Node_{_coordinate}";
    }

    public void RemoveOffset(Vector2Int offset)
    {
        var position = transform.position;
        Vector2Int coordinate = new Vector2Int((int)position.x, (int)position.y);
        _coordinate = coordinate - offset;
        AssignName();
    }

    public void RegisterMatrixNode(MatrixNode matrixNode)
    {
        _matrixNode = matrixNode;
        _hasMatrixNode = true;
        matrixNode.SetNode(this);
    }

    public void UnRegisterMatrixNode()
    {
        _matrixNode.UnsetNode();
        _matrixNode = null;
        _hasMatrixNode = false;
    }

    public void SortingLayerUp()
    {
        renderer.sortingOrder++;
    }
    public void SortingLayerDown()
    {
        renderer.sortingOrder--;
    }


    public void SetColor(Color pieceColor)
    {
        _color = pieceColor;
        renderer.material.color = pieceColor;
    }

    public void SetPiece(Piece piece)
    {
        _piece = piece;
        transform.SetParent(_piece.transform);
        _hasPiece = true;
    }

    public void RemoveFromNode()
    {
        transform.SetParent(null);
        _hasPiece = false;
        _piece = null;
    }


   
}