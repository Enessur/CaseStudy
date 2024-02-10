using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static Action onLevelReset;
    private void OnEnable()
    {
        GridTable.onAllNodesOccupied += OnAllNodesOccupied;       
    }
    private void OnDisable()
    {
        GridTable.onAllNodesOccupied -= OnAllNodesOccupied;
    }
   
    private void OnAllNodesOccupied()
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