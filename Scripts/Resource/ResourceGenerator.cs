using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    
    private float _timer;
    private float _timerMax;
    private ResourceGeneratorData _resourceGeneratorData;
    
    private void Awake()
    {
        _resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        _timerMax = _resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        var nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0)
        {
            //No resource nodes nearby
            //Disable resource amount
            enabled = false;
        }
        else
        {
            _timerMax = (_resourceGeneratorData.timerMax / 2f) + _resourceGeneratorData.timerMax *
                (1 - (float)nearbyResourceAmount / _resourceGeneratorData.maxResourceAmount);
        }
    }

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray =
            Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (var collider2d in collider2DArray)
        {
            var resourceNode = collider2d.GetComponent<ResourceNode>();
            if (resourceNode && resourceGeneratorData.resourceType == resourceNode.resourceType)
            {
                //It's resource node!
                nearbyResourceAmount++;
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
        return nearbyResourceAmount;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = _timerMax;
            ResourceManager.Instance.AddResource(_resourceGeneratorData.resourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return _resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return _timer / _timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / _timerMax;
    }
}
