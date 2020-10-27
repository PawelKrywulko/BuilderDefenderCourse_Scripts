using System;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float edgeScrollingSize = 30f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    public static CameraHandler Instance { get; private set; }

    private float _ortographicSize;
    private float _minOrtographicSize = 10f;
    private float _maxOrtographicSize = 30f;
    private bool _edgeScrolling;

    private void Awake()
    {
        Instance = this;
        _edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }

    private void Start()
    {
        _ortographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (_edgeScrolling)
        {
            HandleEdgeScrolling(ref x, ref y);
        }

        Vector3 moveDirection = new Vector3(x, y).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void HandleEdgeScrolling(ref float x, ref float y)
    {
        if (Input.mousePosition.x + edgeScrollingSize > Screen.width)
        {
            x = 1f;
        }

        if (Input.mousePosition.x < edgeScrollingSize)
        {
            x = -1f;
        }

        if (Input.mousePosition.y + edgeScrollingSize > Screen.height)
        {
            y = 1f;
        }

        if (Input.mousePosition.y < edgeScrollingSize)
        {
            y = -1f;
        }
    }

    private void HandleZoom()
    {
        _ortographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        _ortographicSize = Mathf.Clamp(_ortographicSize, _minOrtographicSize, _maxOrtographicSize);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize,
            _ortographicSize, Time.deltaTime * zoomSpeed);
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        _edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling()
    {
        return _edgeScrolling;
    }
}
