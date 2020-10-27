using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private BuildingTypeSo _buildingType;
    private Transform _buildingDemolishButton;
    private Transform _buildingRepairButton;

    private void Awake()
    {
        _buildingDemolishButton = transform.Find("pf Building Demolish Button");
        _buildingRepairButton = transform.Find("pf Building Repair Button");
        _buildingDemolishButton?.gameObject.SetActive(false);
        _buildingRepairButton?.gameObject.SetActive(false);
    }

    private void Start()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.SetHealthAmountMax(_buildingType.healthAmountMax, true);
        
        _healthSystem.OnDied += HealthSystemOnDied;
        _healthSystem.OnDamaged += HealthSystemOnDamaged;
        _healthSystem.OnHealed += HealthSystemOnHealed;
    }

    private void HealthSystemOnHealed(object sender, EventArgs e)
    {
        if (_healthSystem.IsHealthFull())
        {
            _buildingRepairButton?.gameObject.SetActive(false);
        }
    }

    private void HealthSystemOnDamaged(object sender, EventArgs e)
    {
        _buildingRepairButton?.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        CinemachineShake.Instance.ShakeCamera(7f, 0.15f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void HealthSystemOnDied(object sender, EventArgs e)
    {
        Instantiate(GameAssets.Instance.pfBuildingDestroyedParticles, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        CinemachineShake.Instance.ShakeCamera(10f, 0.2f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        _buildingDemolishButton?.gameObject.SetActive(true);
        if (!_healthSystem.IsHealthFull())
        {
            _buildingRepairButton?.gameObject.SetActive(true);
        }
    }
    
    private void OnMouseExit()
    {
        _buildingDemolishButton?.gameObject.SetActive(false);
        _buildingRepairButton?.gameObject.SetActive(false);
    }
}
