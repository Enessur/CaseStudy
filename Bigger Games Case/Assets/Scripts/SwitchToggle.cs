using System;
using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;

public class SwitchToggle : MonoBehaviour 
{
    
    [SerializeField] RectTransform uiHandleRectTransform ;
    [SerializeField] Color backgroundActiveColor ;
    [SerializeField] Color handleActiveColor ;

    private Image _backgroundImage, _handleImage ;
    private Color _backgroundDefaultColor, _handleDefaultColor ;
    private Toggle _toggle ;
    private Vector2 _handlePosition ;

    void Awake ( ) {
        _toggle = GetComponent <Toggle> ( ) ;

        _handlePosition = uiHandleRectTransform.anchoredPosition ;

        _backgroundImage = uiHandleRectTransform.parent.GetComponent <Image> ( ) ;
        _handleImage = uiHandleRectTransform.GetComponent <Image> ( ) ;

        _backgroundDefaultColor = _backgroundImage.color ;
        _handleDefaultColor = _handleImage.color ;

        _toggle.onValueChanged.AddListener (OnSwitch) ;
        _toggle.isOn = true;

        if (_toggle.isOn)
            OnSwitch (true) ;
    }
    
    void OnSwitch (bool on) {
        uiHandleRectTransform.DOAnchorPos (on ? _handlePosition * -1 : _handlePosition, .4f).SetEase (Ease.InOutBack) ;
        _backgroundImage.DOColor (on ? backgroundActiveColor : _backgroundDefaultColor, .6f) ;
        _handleImage.DOColor (on ? handleActiveColor : _handleDefaultColor, .4f) ;
    }
    void OnDestroy ( ) {
        _toggle.onValueChanged.RemoveListener (OnSwitch) ;
    }
}