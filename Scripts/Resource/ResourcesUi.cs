using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUi : MonoBehaviour
{
    private ResourceTypeListSo _resourceTypeList;
    private readonly Dictionary<ResourceTypeSo, Transform> _resourceTypeTransformDictionary = new Dictionary<ResourceTypeSo, Transform>();
    
    private void Awake()
    {
        _resourceTypeList = Resources.Load<ResourceTypeListSo>(nameof(ResourceTypeListSo));

        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);
        
        foreach (var resourceType in _resourceTypeList.list)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);
            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            
            _resourceTypeTransformDictionary.Add(resourceType, resourceTransform);
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManagerOnResourceAmountChanged;
        UpdateResourceAmount();
    }

    private void ResourceManagerOnResourceAmountChanged(object sender, EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (var resourceType in _resourceTypeList.list)
        {
            Transform resourceTransform = _resourceTypeTransformDictionary[resourceType];
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().text =
                ResourceManager.Instance.GetResourceAmount(resourceType).ToString();
        }
    }
}
