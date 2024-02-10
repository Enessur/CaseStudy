using System.Collections.Generic;
using System.Linq;
using Lean.Touch;
using UnityEngine;

public class Node : PuzzleItem
{
    [SerializeField] private Renderer renderer;
    public Color Color => _color;
    public Piece Piece => _piece;
    public bool HasPiece => _hasPiece;
    private Color _color;
    private Piece _piece;
    private bool _hasPiece;
    private MatrixNode _matrixNode;
    private List<MatrixNode> _matrixNodes = new();
    private bool _hasMatrixNode;

    public void Init()
    {
    }

     
    
    public void RegisterMatrixNode(MatrixNode matrixNode)
    {
        _matrixNode = matrixNode;
        _hasMatrixNode = true;
        //her bir node haangi matrix node a karşılık geldiğini tutması lazım 
    }

    public void UnRegisterMatrixNode()
    {
        _matrixNode = null;
        _hasMatrixNode = false;
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
    

    public bool CanPlace()
    {
        if (!_matrixNodes.Any())
        {
            return false;
        }
        foreach (var matrixNode in _matrixNodes)
        {
            return false;
        }
        return true;
    }
}