using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Building Type")]
public class BuildingTypeSo : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public float constructionTimerMax;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach (var resourceAmount in constructionResourceCostArray)
        {
            str += $"<color=#{resourceAmount.resourceType.colorHex}>{resourceAmount.resourceType.nameShort}:{resourceAmount.amount}</color> ";
        }

        return str;
    }
}
