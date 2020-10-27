using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private Building building; 
    
    private Button _demolishButton;
    
    private void Awake()
    {
        _demolishButton = transform.Find("button").GetComponent<Button>();
        _demolishButton.onClick.AddListener(() =>
        {
            BuildingTypeSo buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;
            foreach (var resourceAmount in buildingType.constructionResourceCostArray)
            {
                ResourceManager.Instance.AddResource(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * 0.6f));
            }
            Destroy(building.gameObject);
        });
    }
}
