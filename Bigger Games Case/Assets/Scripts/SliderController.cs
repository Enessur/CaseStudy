using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Vector2Int sliderMinMax;
    public Action<int> onSliderValueChanged;
    private int _currentValue;

    void Start()
    {
        slider.minValue = sliderMinMax.x;
        slider.maxValue = sliderMinMax.y;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    

    private void OnSliderValueChanged(float value)
    {
        int snappedValue = Mathf.RoundToInt(slider.value);
         
        onSliderValueChanged?.Invoke(snappedValue);
    }
}