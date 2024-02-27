using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GridCanvasLevelButtonPair
{
    public Button levelButton;
    public Vector2Int levelSize;
}

public class GridCanvasController : MonoBehaviour
{
    [SerializeField] private GridCanvasLevelButtonPair[] gridCanvasLevelButtonPairs;
    [SerializeField] private Vector2Int currentValue;
    [SerializeField] private SmoothCam smoothCam;
    
    public static Action CloseMenuOnSelection;


    private void Start()
    {
        foreach (var p in gridCanvasLevelButtonPairs)
        {
            void ButtonClicked() => OnNewLevelCreate(p.levelSize);
            p.levelButton.onClick.AddListener(ButtonClicked);
        }
    }

  
    
    private void OnDestroy()
    {
        foreach (var p in gridCanvasLevelButtonPairs)
        {
            void ButtonClicked() => OnNewLevelCreate(p.levelSize);
            p.levelButton.onClick.RemoveListener(ButtonClicked);
           
        }
    }
    

    public void OnNewLevelCreate(Vector2Int size)
    {
        SoundManager.Instance.PlaySound("SelectLevel");
        PuzzleGenerator.Instance.CreateLevel(size);
        smoothCam.OnGridSizeChange(size);
        CloseMenuOnSelection?.Invoke();

    }
    
}