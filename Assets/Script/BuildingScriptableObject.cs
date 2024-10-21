using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Object", menuName = "BuildingSO")]
public class BuildingScriptableObject : ScriptableObject
{
    public GameObject[] buildingPrefabs;
}
