using System;
using System.Collections;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    [SerializeField] protected LeanSelectableByFinger leanSelectableByFinger;

    public static Action onReloadClicked;
    private readonly WaitForSeconds _endOfSecond = new WaitForSeconds(1f);
    private bool _isFingerDown, _isHoldPressed;
    private Coroutine _holdCoroutine;
    
    private void OnEnable()
    {
        leanSelectableByFinger.OnSelectedFingerUp.AddListener(OnFingerUp);
        leanSelectableByFinger.OnSelectedFinger.AddListener(OnFingerDown);
    }

    private void OnDisable()
    {
        leanSelectableByFinger.OnSelectedFingerUp.RemoveListener(OnFingerUp);
        leanSelectableByFinger.OnSelectedFinger.RemoveListener(OnFingerDown);
    }

    private void OnFingerDown(LeanFinger leanFinger)
    {
        _isFingerDown = true;
        _holdCoroutine = StartCoroutine(HoldCoroutine());
    }

    private void OnFingerUp(LeanFinger leanFinger)
    {
        _isFingerDown = false;
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
        }

        if (!_isFingerDown)
            OnReloadClick();
    }

    private IEnumerator HoldCoroutine()
    {
        _isHoldPressed = false;
        yield return _endOfSecond;

        if (_isFingerDown)
            OnReloadHold();
    }


    public void OnReloadHold()
    {
        _isHoldPressed = true;
        Debug.Log("Reload button held for 1 second");
        _isFingerDown = true;
    }

    public void OnReloadClick()
    {
        if (_isHoldPressed)
        {
            return;
        }

        Debug.Log("Reload button clicked");
        onReloadClicked?.Invoke();
    }
}