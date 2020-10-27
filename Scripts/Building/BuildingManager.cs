using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstructionRadius = 25f;
    public static BuildingManager Instance { get; private set; }
    public event EventHandler<OnActiveBuildingTypeChangeEventArgs> OnActiveBuildingTypeChange;

    public class OnActiveBuildingTypeChangeEventArgs : EventArgs
    {
        public BuildingTypeSo activeBuildingType;
    }
    
    private Camera _mainCamera;
    private BuildingTypeListSo _buildingTypeList;
    private BuildingTypeSo _activeBuildingType;
    private HealthSystem _hqHealthSystem;

    private void Awake()
    {
        Instance = this;
        _buildingTypeList = Resources.Load<BuildingTypeListSo>(nameof(BuildingTypeListSo));
        _activeBuildingType = null;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _hqHealthSystem = hqBuilding.GetComponent<HealthSystem>();
        _hqHealthSystem.OnDied += HqHealthSystemOnDied;
    }

    private void HqHealthSystemOnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_activeBuildingType != null)
            {
                if (CanSpawnBuilding(_activeBuildingType, Utilities.GetMouseWorldPosition(), out string errorMessage))
                {
                    if (ResourceManager.Instance.CanAfford(_activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(_activeBuildingType.constructionResourceCostArray);
                        //Instantiate(_activeBuildingType.prefab, Utilities.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(Utilities.GetMouseWorldPosition(), _activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show(
                            $"Cannot afford {_activeBuildingType.GetConstructionResourceCostString()}", new TooltipUI.TooltipTimer {timer = 2f});
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer {timer = 2f});
                }
            }
            else
            {
                TooltipUI.Instance.Show("Choose building type first!", new TooltipUI.TooltipTimer {timer = 2f});
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSo buildingType)
    {
        _activeBuildingType = buildingType;
        OnActiveBuildingTypeChange?.Invoke(this,
            new OnActiveBuildingTypeChangeEventArgs {activeBuildingType = _activeBuildingType});
    }

    public BuildingTypeSo GetActiveBuildingType()
    {
        return _activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSo buildingType, Vector2 position, out string errorMessage)
    {
        var boxCollider2d = buildingType.prefab.GetComponent<BoxCollider2D>();
        
        var colliders2dArray = Physics2D.OverlapBoxAll(position + boxCollider2d.offset, boxCollider2d.size, 0);
        bool isAreaClear = colliders2dArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        colliders2dArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        foreach (var collider2D in colliders2dArray)
        {
            //Colliders inside the construction radius
            var buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder && buildingTypeHolder.buildingType == buildingType)
            {
                //There's already building type within the construction radius
                errorMessage = "Too close to another building of the same type!";
                return false;
            }
        }

        if (buildingType.hasResourceGeneratorData)
        {
            var resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }
        
        colliders2dArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (var collider2D in colliders2dArray)
        {
            //Colliders inside the construction radius
            var buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder)
            {
                //It's not too far from other buildings on the map
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far from any other building!";
        return false;
    }

    public Building GetHqBuilding()
    {
        return hqBuilding ? hqBuilding : null;
    }
}
