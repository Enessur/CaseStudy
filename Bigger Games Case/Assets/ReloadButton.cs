using System;
using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    [SerializeField] private Button button;
    public static Action onReloadClicked;


    private void OnEnable()
    {
        button.onClick.AddListener(OnReloadClick);
    }
    
    private void OnDisable()
    {
        button.onClick.RemoveListener(OnReloadClick);
    }

    private void OnReloadClick()
    {
        onReloadClicked?.Invoke();
    }
}