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

public enum ModelName
{
    Mall,
    PoorSmallShop,
    NormalSmallShop,
    RichSmallShop,
    PoorBigShop,
    NormalBigShop,
    RichBigShop,
    TinyApartment,
    SmallApartment,
    NormalApartment,
    BigApartment
}

[Serializable]
public struct ModelType
{
    public ModelName modelName;
    public GameObject modelGO;
}

[Serializable]
public struct NestedPrefabCategory
{
    public NestedPrefabCategoryName categoryNameNested;
    public string pathFolder;
    public List<ModelType> modelTypes;

}


[CreateAssetMenu(menuName = "Prefab Creator/Create Nested Prefab Category Settings", fileName = "NestedPrefabCategory")]
public class NestedPrefabCategorySettings : ScriptableObject
{
    public NestedPrefabCategory mallNestPrefab;
    public NestedPrefabCategory shopNestPrefab;
    public NestedPrefabCategory apartmentNestPrefab;

}
