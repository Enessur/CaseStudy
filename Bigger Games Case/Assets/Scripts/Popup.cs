using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button closeButton;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease;
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
        content.SetActive(false);

    }

    public void Open()
    {
        content.SetActive(true);
        content.transform.DOScale(_startScale, duration).SetEase(ease);
    }

    public void Close()
    {
        content.transform.DOScale(0, duration).SetEase(ease).OnComplete(() => { content.SetActive(false); });
    }
    
    
    private void OnCloseClick()
    {
        Close();
    }
}