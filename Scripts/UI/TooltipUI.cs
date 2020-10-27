using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }
    
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private Vector2 padding = new Vector2(8,8);
    
    private RectTransform _rectTransform;
    private TextMeshProUGUI _textMeshPro;
    private RectTransform _backgroundRectTransform;
    private TooltipTimer _tooltipTimer;
    private bool _isActive = false;

    private void Awake()
    {
        Instance = this;
        _rectTransform = GetComponent<RectTransform>();
        _textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        _backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        Hide();
    }

    private void Update()
    {
        if (!_isActive) return;
        
        KeepOnScreen();
        if (_tooltipTimer != null)
        {
            _tooltipTimer.timer -= Time.deltaTime;
            if (_tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    private void KeepOnScreen()
    {
        var anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, 0, canvasRectTransform.rect.width - _backgroundRectTransform.rect.width);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0, canvasRectTransform.rect.height - _backgroundRectTransform.rect.height);
        _rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        _textMeshPro.SetText(tooltipText);
        _textMeshPro.ForceMeshUpdate();
        Vector2 textSize = _textMeshPro.GetPreferredValues(tooltipText);
        _backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        _tooltipTimer = tooltipTimer;
        SetText(tooltipText);
        KeepOnScreen();
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
