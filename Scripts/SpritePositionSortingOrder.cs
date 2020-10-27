using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private float precisionMultiplier = 5f;
    [SerializeField] private float positionOffsetY;
    [SerializeField] private bool runOnce;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _spriteRenderer.sortingOrder = -(int)( (transform.position.y + positionOffsetY) * precisionMultiplier);

        if (runOnce)
        {
            Destroy(this);
        }
    }
}
