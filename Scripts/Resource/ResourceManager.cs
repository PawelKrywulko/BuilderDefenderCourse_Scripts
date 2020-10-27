using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private List<ResourceAmount> startingResourceAmountList;
    
    public static ResourceManager Instance { get; private set; }
    public event EventHandler OnResourceAmountChanged;
    private readonly Dictionary<ResourceTypeSo, int> _resourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();

    private void Awake()
    {
        Instance = this;
        
        ResourceTypeListSo resourceTypeList = Resources.Load<ResourceTypeListSo>(nameof(ResourceTypeListSo));

        foreach (var resourceType in resourceTypeList.list)
        {
            _resourceAmountDictionary.Add(resourceType, 0);
        }

        foreach (var resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }

    private void TestLogResourceAmountDictionary()
    {
        foreach (var resourceType in _resourceAmountDictionary.Keys)
        {
            print($"{resourceType.nameString} : {_resourceAmountDictionary[resourceType]}");
        }
    }

    public void AddResource(ResourceTypeSo resourceType, int amount)
    {
        _resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSo resourceType)
    {
        return _resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (var resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
                //Can afford
            }
            else
            {
                //Can't afford
                return false;
            }
        }
        
        //Can afford all
        return true;
    }
    
    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (var resourceAmount in resourceAmountArray)
        {
            _resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
