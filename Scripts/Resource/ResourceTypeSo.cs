using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Resource Type")]
public class ResourceTypeSo : ScriptableObject
{
    public string nameString;
    public string nameShort;
    public Sprite sprite;
    public string colorHex;
}
