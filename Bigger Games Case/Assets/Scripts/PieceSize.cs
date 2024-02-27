using System;
using UnityEngine;

[Serializable]
public class PieceSize
{
    public Piece piece;
    public int pieceSizeX;
    public int pieceSizeY;

    public PieceSize(Piece piece, int pieceSizeX, int pieceSizeY)
    {
        this.piece = piece;
        this.pieceSizeX = pieceSizeX;
        this.pieceSizeY = pieceSizeY;
    }

}