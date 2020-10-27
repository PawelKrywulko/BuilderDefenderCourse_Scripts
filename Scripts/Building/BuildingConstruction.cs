using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float _constructionTimer;
    private BuildingTypeSo _buildingType;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private BuildingTypeHolder _buildingTypeHolder;
    private Material _constructionMaterial;
    private static readonly int Progress = Shader.PropertyToID("_Progress");

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        _buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        _constructionMaterial = _spriteRenderer.material;

        Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        _constructionTimer -= Time.deltaTime;
        _constructionMaterial.SetFloat(Progress, GetConstructionTimerNormalized());
        if (_constructionTimer <= 0f)
        {
            Instantiate(_buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }
    }
    
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSo buildingType)
    {
        var pfBuildingConstruction = GameAssets.Instance.pfBuildingConstruction;
        var buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);
        var buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);
        return buildingConstruction;
    }

    private void SetBuildingType(BuildingTypeSo buildingType)
    {
        _buildingType = buildingType;
        _constructionTimer = buildingType.constructionTimerMax;
        _boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        _boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
        _spriteRenderer.sprite = buildingType.sprite;
        _buildingTypeHolder.buildingType = buildingType;
    }

    public float GetConstructionTimerNormalized()
    {
        return 1 - _constructionTimer / _buildingType.constructionTimerMax;
    }
}
