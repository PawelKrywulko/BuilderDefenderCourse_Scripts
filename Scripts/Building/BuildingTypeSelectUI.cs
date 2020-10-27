using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private List<BuildingTypeSo> ignoreBuildingTypeList;
    private BuildingTypeListSo _buildingTypeList;
    private readonly Dictionary<BuildingTypeSo, Transform> _buttonTransformDictionary = new Dictionary<BuildingTypeSo, Transform>();
    
    private void Awake()
    {
        _buildingTypeList = Resources.Load<BuildingTypeListSo>(nameof(BuildingTypeListSo));
        
        Transform buttonTemplate = transform.Find("buttonTemplate");
        buttonTemplate.gameObject.SetActive(false);

        foreach (var buildingType in _buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);
            buttonTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;
            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                HandleSelectedButton(buildingType);
            });

            AssignMouseEvents(buttonTransform, buildingType);
            _buttonTransformDictionary.Add(buildingType, buttonTransform);
        }
    }

    private static void AssignMouseEvents(Transform buttonTransform, BuildingTypeSo buildingType)
    {
        var mouseEnterExitEvents = buttonTransform.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (sender, e) =>
        {
            TooltipUI.Instance.Show($"{buildingType.nameString}\n{buildingType.GetConstructionResourceCostString()}");
        };
        mouseEnterExitEvents.OnMouseExit += (sender, e) => { TooltipUI.Instance.Hide(); };
    }

    private void Start()
    {
        UpdateActiveBuildingTypeButton();
    }
    
    private void HandleSelectedButton(BuildingTypeSo buildingType) 
    {
        if(BuildingManager.Instance.GetActiveBuildingType() == buildingType) 
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
            UpdateActiveBuildingTypeButton();
        }
        else {
            BuildingManager.Instance.SetActiveBuildingType(buildingType);
            UpdateActiveBuildingTypeButton();
        }
    }

    private void UpdateActiveBuildingTypeButton()
    {
        foreach (var buildingType in _buttonTransformDictionary.Keys)
        {
            _buttonTransformDictionary[buildingType].Find("selected").gameObject.SetActive(false);
        }
        
        var activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (!activeBuildingType) return;
        _buttonTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
    }
}
