﻿using System;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEditor;
using UnityEngine;

public class PieceUndo : Singleton<PieceUndo>
{
    private List<UndoData> _pieceData = new();
    private Piece piece;
    private UndoData _item;
    public static Action onLevelReload;

    //  public static Action<UndoData> onUndo;

    private void OnEnable()
    {
        ReloadButton.onReloadClicked += UndoMove;
        ReloadButton.onReloadHold += OnReloadHold;
        Piece.onPiecePlaced += AddMove;
        Piece.onPieceRemove += RemovePiece;
    }

    private void OnDisable()
    {
        ReloadButton.onReloadClicked -= UndoMove;
        ReloadButton.onReloadHold -= OnReloadHold;
        Piece.onPiecePlaced -= AddMove;
        Piece.onPieceRemove -= RemovePiece;
    }

    public void AddMove(Piece piece, Vector3 position)
    {
        _pieceData.Add(new UndoData(piece, position));
        Debug.Log($"Piece :{piece} Position :{position}");
    }

    public void UndoMove()
    {
        if (!_pieceData.Any())
        {
            return;
        }

        var data = _pieceData.Last();
        _pieceData.Remove(data);
        UndoPiece(data);
        //  onUndo?.Invoke(data);
    }

    public void UndoPiece(UndoData undoData)
    {
        piece = undoData.piece;
        piece.Undo(undoData.position);
    }


    public void RemovePiece(Piece piece)
    {
        Debug.Log("remove piece");
        var c = _pieceData.Count;
        for (int i = c; i < 0; i--)
        {
            if (piece == _pieceData[i].piece)
            {
                _pieceData.RemoveAt(i);
            }
        }
    }

    public void ClearList()
    {
        _pieceData.Clear();
    }

    private void OnReloadHold()
    {
        onLevelReload?.Invoke();
        ClearList();
    }
}