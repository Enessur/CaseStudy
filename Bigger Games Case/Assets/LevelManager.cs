using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PuzzleGenerator puzzleGenerator;
    private List<Piece> _pieces = new();
    private int _puzzleSize;
    private int _piecesPlaced = 0;
    private bool _isCheckOn;
    private void Start()
    {
        _isCheckOn = true;
        _puzzleSize = puzzleGenerator.puzzleSize;
    }

    private void OnEnable()
    {
        GridPlane.OnEnterAction += CorrectPieceCount;
        GridPlane.OnExitAction += RemovePiece;
    }

    private void OnDisable()
    {
        GridPlane.OnEnterAction -= CorrectPieceCount;
        GridPlane.OnExitAction -= RemovePiece;
    }


    private void CorrectPieceCount()
    {
        _piecesPlaced++;
        Debug.Log(_piecesPlaced);
        if (_piecesPlaced == _puzzleSize * _puzzleSize)
        {
            Debug.Log("Win!");
            
            ResetPuzzle();
           
        }
    }

    
    private void RemovePiece()
    {
        _piecesPlaced--;
    }


    private void ResetPuzzle()
    {
        _pieces.RemoveAll(x => x == null);
        
        foreach (Piece piece in _pieces)
        {
            if (piece.transform.childCount > 0)
            {
                
                for (int i = 0; i <piece.transform.childCount; i++)
                {
                    Destroy(piece.transform.GetChild(i).gameObject);
                }
            }
        }
        
        foreach (Transform child in puzzleGenerator.transform)
        {
            Destroy(child.gameObject);
        }

        puzzleGenerator.GeneratePuzzle();
        _piecesPlaced = 0;
    }
}