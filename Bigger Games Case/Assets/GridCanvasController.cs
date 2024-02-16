using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct GridCanvasLevelButtonPair
{
    public Button levelButton;
    public Vector2Int levelSize;
}

[Serializable]
public struct GridCanvasLevelSliderPair
{
   
    public SliderController sliderXY;
}

public class GridCanvasController : MonoBehaviour
{
    [SerializeField] private GridCanvasLevelButtonPair[] gridCanvasLevelButtonPairs;
    [SerializeField] private GridCanvasLevelSliderPair gridCanvasLevelSliderPair;
    [SerializeField] private Vector2Int currentValue;
    [SerializeField] private SmoothCam smoothCam;
    
    public static Action CloseMenuOnSelection;
    private void OnEnable()
    {
        foreach (var p in gridCanvasLevelButtonPairs)
        {
            void ButtonClicked() => OnEasyLevelCreate(p.levelSize);
            p.levelButton.onClick.AddListener(ButtonClicked);
        }
        gridCanvasLevelSliderPair.sliderXY.onSliderValueChanged += OnSliderValueChanged;
         }

     

    private void OnSliderValueChanged(int value)
    {
        currentValue = new Vector2Int(value, value);
        OnCustomLevelCreate(currentValue);
    }

    private void OnDisable()
    {
        gridCanvasLevelSliderPair.sliderXY.onSliderValueChanged -= OnSliderValueChanged;
    }


    public void OnCustomLevelCreate(Vector2Int size)
    {
        PuzzleGenerator.Instance.CreateLevel(size);
        smoothCam.OnGridSizeChange(size);
    }

    public void OnEasyLevelCreate(Vector2Int size)
    {
        PuzzleGenerator.Instance.CreateLevel(size);
        smoothCam.OnGridSizeChange(size);
        CloseMenuOnSelection?.Invoke();
    }

    public void Subscription()
    {
        GridTable.onGridCreate += AdjustCanvas;
    }

    public void Unsubscription()
    {
        GridTable.onGridCreate -= AdjustCanvas;
    }

    private void AdjustCanvas()
    {
    }
}