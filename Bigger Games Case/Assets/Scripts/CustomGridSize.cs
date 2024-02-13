using UnityEngine;
using UnityEngine.UI;

public class CustomGridSize : MonoBehaviour
{
    [SerializeField] private PuzzleGenerator puzzleGenerator;
    [SerializeField] private Slider sliderXValue;
    [SerializeField] private Slider sliderYValue;
    [SerializeField] private SmoothCam smoothCam;
    

    public int GridXValue => _gridXValue;
    public int GridYValue => _gridYValue;
    
    private int _gridXValue, _gridYValue;
    private int _gridSizeMax = 12,_gridSizeMin = 4;
    private int _gridOrginPoint; 

    private void Start()
    {
        sliderXValue.onValueChanged.AddListener(OnSliderValueChanged);
        sliderYValue.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _gridXValue = LerpThis(_gridSizeMin, _gridSizeMax, sliderXValue.value);
        _gridYValue = LerpThis(_gridSizeMin, _gridSizeMax, sliderYValue.value);
        Debug.Log($"Slider X value: {_gridXValue} Slider Y value:{_gridYValue}");
        smoothCam.OnGridSizeChange();
    }
    

    private int LerpThis(int gridMin, int gridMax, float sliderValue)
    {
    
        var value = (int)Mathf.Lerp(gridMin, gridMax, sliderValue);
        return value;
    }
}