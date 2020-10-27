using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject _spriteGameObject;
    private ResourceNearbyOverlay _resourceNearbyOverlay;
    
    private void Awake()
    {
        _spriteGameObject = transform.Find("sprite").gameObject;
        _resourceNearbyOverlay = transform.Find("pf Resource Nearby Overlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManagerOnActiveBuildingTypeChange;
    }

    private void BuildingManagerOnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventArgs e)
    {
        if (e.activeBuildingType is null)
        {
            Hide();
            _resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGeneratorData)
            {
                _resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                _resourceNearbyOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        transform.position = Utilities.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        _spriteGameObject.SetActive(true);
        _spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        _spriteGameObject.SetActive(false);
    }
}
