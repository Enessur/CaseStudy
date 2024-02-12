using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static Action onLevelReset;
    private void OnEnable()
    {
        PuzzleGenerator.OnAllPiecesPlaced += OnAllPiecesPlaced;
    }
    private void OnDisable()
    {
        PuzzleGenerator.OnAllPiecesPlaced -= OnAllPiecesPlaced;
    }
   
    private void OnAllPiecesPlaced()
    {
        ResetLevelWithAnimation();
    }

    private void ResetLevelWithAnimation()
    {
        ResetLevel();
    }

    private void ResetLevel()
    {
        onLevelReset?.Invoke();
    }
}