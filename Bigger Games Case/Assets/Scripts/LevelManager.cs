using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static Action onLevelReset;
    public static Action onLevelResetAnimation;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

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
        StartCoroutine(StartAnimationRoutine());
    }

    private IEnumerator StartAnimationRoutine()
    {
        onLevelResetAnimation?.Invoke();
        yield return new WaitForSeconds(LevelEndingAnimator.GetNextLevelDuration());
        ResetLevel();
    }

    private void ResetLevel()
    {
        onLevelReset?.Invoke();
    }
}