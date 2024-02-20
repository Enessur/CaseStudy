using System;
using System.Collections.Generic;
using Script;

public class PieceStack :Singleton<PieceStack>
{
      private Stack<Piece> pieceStack;

      private void OnEnable()
      {
            ReloadButton.onReloadClicked += Clear;
      }

      private void OnDisable()
      {
            ReloadButton.onReloadClicked -= Clear;
      }

      public PieceStack()
      {
            pieceStack = new Stack<Piece>();
      }

      public void Push(Piece piece)
      {
            pieceStack.Push(piece);
      }

      public Piece Pop()
      {
            if (pieceStack.Count == 0)
            {
                  return null;
            }

            return pieceStack.Pop();
      }

      public void Clear()
      {
            pieceStack.Clear();
      }
}