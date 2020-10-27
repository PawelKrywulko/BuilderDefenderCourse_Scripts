using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Building Type List")]
public class BuildingTypeListSo : ScriptableObject
{
    public List<BuildingTypeSo> list;
}
