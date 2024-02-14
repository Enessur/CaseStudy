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
    // public SliderController sliderX;
    // public SliderController sliderY;
    public SliderController sliderXY;
}

public class GridCanvasController : MonoBehaviour
{
    [SerializeField] private GridCanvasLevelButtonPair[] gridCanvasLevelButtonPairs;
    [SerializeField] private GridCanvasLevelSliderPair gridCanvasLevelSliderPair;
    [SerializeField] private Vector2Int currentValue;
    [SerializeField] private SmoothCam smoothCam;
    
    private void OnEnable()
    {
        foreach (var p in gridCanvasLevelButtonPairs)
        {
            void ButtonClicked() => OnEasyLevelCreate(p.levelSize);
            p.levelButton.onClick.AddListener(ButtonClicked);
        }
        gridCanvasLevelSliderPair.sliderXY.onSliderValueChanged += OnSliderValueChanged;
        
        // gridCanvasLevelSliderPair.sliderX.onSliderValueChangedInt += OnSliderValueChangedInt;
        // gridCanvasLevelSliderPair.sliderY.onSliderValueChangedInt += OnSliderValueChangedInt;
    }

    // private void OnSliderValueChangedInt(int value, bool isX)
    // {
    //     if (isX)
    //     {
    //         currentValue = new Vector2Int(value, currentValue.y);
    //     }
    //     else
    //     {
    //         currentValue = new Vector2Int(currentValue.x, value);
    //     }
    //     OnCustomLevelCreate(currentValue);
    //   
    // }

    private void OnSliderValueChanged(int value)
    {
        currentValue = new Vector2Int(value, value);
        OnCustomLevelCreate(currentValue);
    }

    private void OnDisable()
    {
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