using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;


[Serializable]
public enum NestedPrefabCategoryName
{
    Shop,
    Mall,
    Apartment
}

[Serializable]
public struct NestedPrefabCategory
{
    public NestedPrefabCategoryName categoryNameNested;
    public string pathFolder;
    public GameObject obstructionModel;

}

[CreateAssetMenu(menuName = "Prefab Creator/Create Nested Prefab Category Settings", fileName = "NestedPrefabCategory")]
public class NestedPrefabCategorySettings : ScriptableObject
{
    public NestedPrefabCategory shopNestPrefab;
    public NestedPrefabCategory mallNestPrefab;
    public NestedPrefabCategory apartmentNestPrefab;

}
