using System;
using UnityEngine;

[Serializable]
public class UndoData
{
    public Piece piece;
    public Vector3 position;

    public UndoData(Piece piece, Vector3 position)
    {
        this.piece = piece;
        this.position = position;
    }
}