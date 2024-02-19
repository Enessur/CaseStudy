using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button closeButton;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease;

    private bool _isOpen;
    private Vector3 _startScale;

    private void OnEnable()
    {
        GridCanvasController.CloseMenuOnSelection += Close;
        closeButton.onClick.AddListener(OnCloseClick);
    }

    private void OnDisable()
    {
        GridCanvasController.CloseMenuOnSelection -= Close;
        closeButton.onClick.RemoveListener(OnCloseClick);
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _startScale = content.transform.localScale;
        _isOpen = false;
        content.SetActive(false);

    }

    public void Open()
    {
        if (_isOpen)
        {
            return;
        }

        content.SetActive(true);
        content.transform.DOScale(_startScale, duration).SetEase(ease);
        _isOpen = true;
    }

    public void Close()
    {
        if (!_isOpen)
        {
            return;
        }

        content.transform.DOScale(0, duration).SetEase(ease).OnComplete(() => { content.SetActive(false); });
        _isOpen = false;
    }
    
    
    private void OnCloseClick()
    {
        Close();
    }
}