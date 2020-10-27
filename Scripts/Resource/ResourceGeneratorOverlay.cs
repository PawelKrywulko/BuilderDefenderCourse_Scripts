using TMPro;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;

    private ResourceGeneratorData _resourceGeneratorData;
    private Transform _barTransform;
    
    private void Start()
    {
        _resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();
        _barTransform = transform.Find("bar");
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = _resourceGeneratorData.resourceType.sprite;
        transform.Find("text").GetComponent<TextMeshPro>().text = resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1");
    }

    private void Update()
    {
        _barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(),1,1);
    }
}
