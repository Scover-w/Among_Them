using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum WealthLevelShop
{
    Poor,
    Normal,
    Rich
}



public enum SizeShop
{
    Small,
    Big
}

public enum SizeApartment
{
    Tiny,
    Small,
    Medium,
    Big
}

public class NestedPrefabCategoryEditorWindow : EditorWindow
{
    private const string pathToSetting = "Assets/Scripts/Editor/NestedPrefabCategorySettings.asset";
    private NestedPrefabCategorySettings settings;
    private NestedPrefabCategory selectedNestedPrefabCategory;

    private GameObject prefabParent;
    private List<GameObject> objectsInScene = new List<GameObject>();
    private int nbOldObjects = 1;
    private GameObject roomModel;

    private WealthLevelShop selectedWealthLevelShop = WealthLevelShop.Poor;
    private SizeShop selectedSizeShop = SizeShop.Small;
    private SizeApartment seletectedSizeApartment = SizeApartment.Tiny;

    private ModelType selectedModelType;

    private int nbObjectsBeforeNoParentPrefab;

    private bool onFirstOnGUI = true;
    
    [MenuItem("Tools/Open Prefab  Window")]
    static void Init()
    {
        var wd = GetWindow(typeof(NestedPrefabCategoryEditorWindow));
        wd.Show();
    }

    private void OnDestroy()
    {
        ClearObjects();
    }

    // /!\ Called two times when drag&drop a prefab :
    private void OnHierarchyChange()
    {
        var actualObjects = FindObjectsOfType(typeof(GameObject));
        var nbActualObjects = actualObjects.Length;

        if (Selection.activeTransform == null) // Prefab has been removed or changement in hierarchy
        {
            if (nbActualObjects < nbOldObjects) // Prefab has been removed
            {
                Selection.activeGameObject = prefabParent;
                if (prefabParent == null)
                {
                    nbOldObjects = 1;
                }
                else
                {
                    nbOldObjects = nbActualObjects;
                }
            }
            // Nothing to handle for changement in the hierarchy

            return;
        }

        if (IsLightOfScene(Selection.activeGameObject) || Selection.activeGameObject == prefabParent)
        {
            return;
        }
        
        
        if (nbActualObjects != nbOldObjects) // Prefab added
        {
            if (nbOldObjects == nbObjectsBeforeNoParentPrefab && nbActualObjects > nbObjectsBeforeNoParentPrefab) // prefabParentAdded
            {
                PrefabParentAdded(Selection.activeTransform.gameObject);
                nbOldObjects = nbActualObjects;
            }
            else if(nbActualObjects > nbOldObjects && !objectsInScene.Contains(Selection.activeGameObject)) // prefabChildAdded
            {
                Undo.RecordObject(Selection.activeGameObject, "Object ObjectChangementParentPrefab");
                Selection.activeTransform.parent = prefabParent.transform;
                Selection.activeTransform.position = Vector3.zero;
                AddChildRecursively(Selection.activeGameObject);
                nbOldObjects = nbActualObjects;
            }
        }
    }

    public void OnGUI()
    {
        if (settings == null)
        {
            settings = AssetDatabase.LoadAssetAtPath<NestedPrefabCategorySettings>(pathToSetting);
            if (settings == null)
            {
                Debug.LogWarning("You need to create the NestedPrefabsCategories to be able to register your nested prefab");
                settings = CreateInstance<NestedPrefabCategorySettings>();
                AssetDatabase.CreateAsset(settings, pathToSetting);
                AssetDatabase.SaveAssets();
            }
        }

        if (onFirstOnGUI)
        {
            GetModelType();
            onFirstOnGUI = false;
        }

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        GUILayout.Label("Catégorie :");
        EditorGUI.indentLevel++;
        if (EditorGUILayout.Toggle("Shop", selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Shop))
        {
            if (selectedNestedPrefabCategory.categoryNameNested != NestedPrefabCategoryName.Shop)
            {
                selectedNestedPrefabCategory = settings.shopNestPrefab;
                GetModelType();
            }
        }

        if (EditorGUILayout.Toggle("Mall", selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Mall))
        {
            if (selectedNestedPrefabCategory.categoryNameNested != NestedPrefabCategoryName.Mall)
            {
                selectedNestedPrefabCategory = settings.mallNestPrefab;
                GetModelType();
            }
        }

        if (EditorGUILayout.Toggle("Apartment", selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Apartment))
        {
            if (selectedNestedPrefabCategory.categoryNameNested != NestedPrefabCategoryName.Apartment)
            {
                selectedNestedPrefabCategory = settings.apartmentNestPrefab;
                GetModelType();
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        if (selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Shop)
        {
            GUILayout.Label("Wealth :");
            EditorGUI.indentLevel++;
            if (EditorGUILayout.Toggle("Poor", selectedWealthLevelShop == WealthLevelShop.Poor))
            {
                if (selectedWealthLevelShop != WealthLevelShop.Poor)
                {
                    selectedWealthLevelShop = WealthLevelShop.Poor;
                    GetModelType();
                }
            }

            if (EditorGUILayout.Toggle("Normal", selectedWealthLevelShop == WealthLevelShop.Normal))
            {
                if (selectedWealthLevelShop != WealthLevelShop.Normal)
                {
                    selectedWealthLevelShop = WealthLevelShop.Normal;
                    GetModelType();
                }
            }
            if (EditorGUILayout.Toggle("Rich", selectedWealthLevelShop == WealthLevelShop.Rich))
            {
                if (selectedWealthLevelShop != WealthLevelShop.Rich)
                {
                    selectedWealthLevelShop = WealthLevelShop.Rich;
                    GetModelType();
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            
            GUILayout.Label("Size :");
            EditorGUI.indentLevel++;
            if (EditorGUILayout.Toggle("Small", selectedSizeShop == SizeShop.Small))
            {
                if (selectedSizeShop != SizeShop.Small)
                {
                    selectedSizeShop = SizeShop.Small;
                    GetModelType();
                }
            }
            if (EditorGUILayout.Toggle("Big", selectedSizeShop == SizeShop.Big))
            {
                if (selectedSizeShop != SizeShop.Big)
                {
                    selectedSizeShop = SizeShop.Big;
                    GetModelType();
                }
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        else if(selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Apartment)
        {
            GUILayout.Label("Size :");
            EditorGUI.indentLevel++;
            if (EditorGUILayout.Toggle("Tiny", seletectedSizeApartment == SizeApartment.Tiny))
            {
                if (seletectedSizeApartment != SizeApartment.Tiny)
                {
                    seletectedSizeApartment = SizeApartment.Tiny;
                    GetModelType();
                }
            }
            if (EditorGUILayout.Toggle("Small", seletectedSizeApartment == SizeApartment.Small))
            {
                if (seletectedSizeApartment != SizeApartment.Small)
                {
                    seletectedSizeApartment = SizeApartment.Small;
                    GetModelType();
                }
            }
            if (EditorGUILayout.Toggle("Normal", seletectedSizeApartment == SizeApartment.Medium))
            {
                if (seletectedSizeApartment != SizeApartment.Medium)
                {
                    seletectedSizeApartment = SizeApartment.Medium;
                    GetModelType();
                }
            }
            if (EditorGUILayout.Toggle("Big", seletectedSizeApartment == SizeApartment.Big))
            {
                if (seletectedSizeApartment != SizeApartment.Big)
                {
                    seletectedSizeApartment = SizeApartment.Big;
                    GetModelType();
                }
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }

        if (settings.shopNestPrefab.categoryNameNested == settings.mallNestPrefab.categoryNameNested &&
            settings.mallNestPrefab.categoryNameNested == settings.apartmentNestPrefab.categoryNameNested &&
            settings.apartmentNestPrefab.categoryNameNested == settings.shopNestPrefab.categoryNameNested)
            GUILayout.Label("Please fill correctly NestedPrefabCategorySettings in the Editor Folder");
        else
        {
            if (GUILayout.Button("Create prefab") && AskCreatePrefab())
            {
                foreach (Transform child in prefabParent.transform)
                {
                    // TODO : Delete on object directly
                    RecursivelyRemoveProceduralEntitiesChildren(child.gameObject);
                }

                string localPath = selectedNestedPrefabCategory.pathFolder;

                if (selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Shop)
                {
                    localPath += selectedWealthLevelShop + "/" + selectedSizeShop + "/";
                }
                else if (selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Apartment)
                {
                    localPath += seletectedSizeApartment + "/";
                }
                else if(selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Mall)
                {
                    localPath += "Mall/";
                }
                
                

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                
                localPath += prefabParent.name + ".prefab";
                
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
                
                PrefabUtility.SaveAsPrefabAssetAndConnect(prefabParent, localPath,
                    InteractionMode.UserAction);
                
                DestroyImmediate(prefabParent);
                
                Debug.Log("Nested prefab created");
            }
        }




    }
    
    private void PrefabParentAdded(GameObject activeGameObject)
    {
        Light light;

        if (activeGameObject != null && !IsLightOfScene(activeGameObject))
        {
            prefabParent = activeGameObject;
            prefabParent.transform.position = Vector3.zero;
            AddChildRecursively(prefabParent);
        }
    }
    
    private void AddChildRecursively(GameObject child)
    {
        objectsInScene.Add(child);
        
        foreach (Transform childSquare in child.transform)
        {
            AddChildRecursively(childSquare.gameObject);
        }
    }

    private bool IsLightOfScene(GameObject gameObject)
    {
        Light light;
        return gameObject.TryGetComponent(out light);
    }

    public bool AskCreatePrefab()
    {
        if (prefabParent == null) // Does prefabParentExist
        {
            Debug.LogError("No prefab in the scene");
            return false;
        }

        if (selectedNestedPrefabCategory.pathFolder == "")
        {
            Debug.LogError("Path not filled");
            return false;
        }

        return true;
    }

    public void InstantiateRoomModel()
    {
        ClearObjects();

        roomModel = Instantiate(selectedModelType.modelGO);
        roomModel.transform.position = Vector3.zero;

        nbOldObjects = FindObjectsOfType(typeof(GameObject)).Length;
        nbObjectsBeforeNoParentPrefab = nbOldObjects;
    }

    public void RecursivelyRemoveProceduralEntitiesChildren(GameObject parent)
    {
        DestroyImmediate(parent.GetComponent<ProceduralEntity>());
        
        foreach (Transform child in parent.transform)
        {
            RecursivelyRemoveProceduralEntitiesChildren(child.gameObject);
        }
    }

    public void GetModelType()
    {
        switch (selectedNestedPrefabCategory.categoryNameNested)
        {
            case NestedPrefabCategoryName.Shop:
                selectedModelType = GetModelTypeShop();
                break;
            case NestedPrefabCategoryName.Mall:
                selectedModelType = GetModelTypeMall();
                break;
            case NestedPrefabCategoryName.Apartment:
                selectedModelType = GetModelTypeApartment();
                break;
        }

        InstantiateRoomModel();
    }

    private ModelType GetModelTypeShop()
    {
        if (selectedSizeShop == SizeShop.Small)
        {
            switch (selectedWealthLevelShop)
            {
                case WealthLevelShop.Poor:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.PoorSmallShop);
                case WealthLevelShop.Normal:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.NormalSmallShop);
                default: // WealthLevelShop.Rich:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.RichSmallShop);
            }
        }
        else
        {
            switch (selectedWealthLevelShop)
            {
                case WealthLevelShop.Poor:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.PoorBigShop);
                case WealthLevelShop.Normal:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.NormalBigShop);
                default: // WealthLevelShop.Rich:
                    return settings.shopNestPrefab.modelTypes.First(x => x.modelName == ModelName.RichBigShop);

            }
        }
        
    }

    private ModelType GetModelTypeMall()
    {
        return settings.mallNestPrefab.modelTypes.First(x => x.modelName == ModelName.Mall);
    }

    private ModelType GetModelTypeApartment()
    {
        switch (seletectedSizeApartment)
        {
            case SizeApartment.Tiny:
                return settings.apartmentNestPrefab.modelTypes.First(x => x.modelName == ModelName.TinyApartment);
            case SizeApartment.Small:
                return settings.apartmentNestPrefab.modelTypes.First(x => x.modelName == ModelName.SmallApartment);
            case SizeApartment.Medium:
                return settings.apartmentNestPrefab.modelTypes.First(x => x.modelName == ModelName.NormalApartment);
            default: // SizeApartment.Big:
                return settings.apartmentNestPrefab.modelTypes.First(x => x.modelName == ModelName.BigApartment);
        }
    }

    private void ClearObjects()
    {
        if(roomModel != null)
            DestroyImmediate(roomModel);
        
        if(prefabParent != null)
            DestroyImmediate(prefabParent);
    }
    
}
