using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSo goldResourceType;
    
    private Button _repairButton;
    
    private void Awake()
    {
        _repairButton = transform.Find("button").GetComponent<Button>();
        _repairButton.onClick.AddListener(() =>
        {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;
            var resourceAmountCost = new[] { new ResourceAmount {resourceType = goldResourceType, amount = repairCost} };
            bool canAfford = ResourceManager.Instance.CanAfford(resourceAmountCost);
            
            if (canAfford)
            {
                //Can afford the repairs
                healthSystem.HealFull();
                ResourceManager.Instance.SpendResources(resourceAmountCost);
            }
            else
            {
                //Cannot afford the repairs
                TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer{timer = 2f});
            }
        });
    }
}
