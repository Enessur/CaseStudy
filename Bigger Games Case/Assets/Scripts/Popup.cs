using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease;
    private bool _isAnimationActive;
    private Vector3 _startScale;

    private void OnEnable()
    {
        GridCanvasController.CloseMenuOnSelection += Close;
        GridTable.onAnimationActive += OnAnimationActive;
        closeButton.onClick.AddListener(OnCloseClick);
        settingsButton.onClick.AddListener(Open);
    }

    private void OnDisable()
    {
        GridCanvasController.CloseMenuOnSelection -= Close;
        closeButton.onClick.RemoveListener(OnCloseClick);
        settingsButton.onClick.RemoveListener(Open);
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _startScale = content.transform.localScale;
        content.SetActive(false);

    }

    public void Open()
    {
        if (_isAnimationActive)
        {
            return;
        }
        content.SetActive(true);
        content.transform.DOScale(_startScale, duration).SetEase(ease);
    }

    public void Close()
    {
        content.transform.DOScale(0, duration).SetEase(ease).OnComplete(() => { content.SetActive(false); });
    }

    public void OnAnimationActive(bool isAnimationActive)
    {
        _isAnimationActive = isAnimationActive;
    }
    
    private void OnCloseClick()
    {
        Close();
    }
}