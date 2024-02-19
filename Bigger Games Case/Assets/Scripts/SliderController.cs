using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector2Int sliderMinMax;
    //[SerializeField] private bool isX;
    //public Action<int, bool> onSliderValueChangedInt;
    public Action<int> onSliderValueChanged;
    private int _currentValue;

    void Start()
    {
        slider.minValue = sliderMinMax.x;
        slider.maxValue = sliderMinMax.y;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        //slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // private void OnSliderValueChanged(float value)
    // {
    //     int snappedValue = Mathf.RoundToInt(slider.value);
    //     slider.value = snappedValue;
    //     onSliderValueChangedInt?.Invoke(snappedValue, isX);
    // }

    private void OnSliderValueChanged(float value)
    {
        int snappedValue = Mathf.RoundToInt(slider.value);
         
        onSliderValueChanged?.Invoke(snappedValue);
    }
}