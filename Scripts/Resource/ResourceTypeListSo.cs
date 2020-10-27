using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Resource Type List")]
public class ResourceTypeListSo : ScriptableObject
{
    public List<ResourceTypeSo> list;
}
