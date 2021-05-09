using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NestedPrefabCategoryEditorWindow : EditorWindow
{
    private const string pathToSetting = "Assets/Scripts/Editor/NestedPrefabCategorySettings.asset";
    private NestedPrefabCategorySettings settings;
    private NestedPrefabCategory selectedNestedPrefabCategory;
    private bool isNestedPrefabCategoryChoosen = false;
    private bool[] exclusiveToggle = new[] {false, false, false};
    
    private GameObject prefabParent;
    private List<GameObject> objectsInScene = new List<GameObject>();
    private int nbOldObjects = 1;
    private GameObject obstructionModel;
    

    [MenuItem("Tools/Open Prefab  Window")]
    static void Init()
    {
        GetWindow(typeof(NestedPrefabCategoryEditorWindow)).Show();;
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
            if (nbOldObjects == 1 && nbActualObjects > 1) // prefabParentAdded
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
        
        if (isNestedPrefabCategoryChoosen)
        {
            exclusiveToggle[0] = selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Shop;
            exclusiveToggle[1] = selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Mall;
            exclusiveToggle[2] = selectedNestedPrefabCategory.categoryNameNested == NestedPrefabCategoryName.Apartment;
        }
        else
        {
            exclusiveToggle[0] = false;
            exclusiveToggle[1] = false;
            exclusiveToggle[2] = false;
        }
        

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        GUILayout.Label("Catégorie :");
        EditorGUI.indentLevel++;
        if (EditorGUILayout.Toggle("Shop", exclusiveToggle[0]))
        {
            if (exclusiveToggle[0] == false)
            {
                isNestedPrefabCategoryChoosen = true;
                selectedNestedPrefabCategory = settings.shopNestPrefab;
                InstantiateObstructionModel();
            }
        }

        if (EditorGUILayout.Toggle("Mall", exclusiveToggle[1]))
        {
            if (exclusiveToggle[1] == false)
            {
                isNestedPrefabCategoryChoosen = true;
                selectedNestedPrefabCategory = settings.mallNestPrefab;
                InstantiateObstructionModel();
            }
        }

        if (EditorGUILayout.Toggle("Apartment", exclusiveToggle[2]))
        {
            if (exclusiveToggle[2] == false)
            {
                isNestedPrefabCategoryChoosen = true;
                selectedNestedPrefabCategory = settings.apartmentNestPrefab;
                InstantiateObstructionModel();
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        if (settings.shopNestPrefab.categoryNameNested == settings.mallNestPrefab.categoryNameNested &&
            settings.mallNestPrefab.categoryNameNested == settings.apartmentNestPrefab.categoryNameNested &&
            settings.apartmentNestPrefab.categoryNameNested == settings.shopNestPrefab.categoryNameNested)
            GUILayout.Label("Please fill correctly NestedPrefabCategorySettings in the Editor Folder");
        else
        {
            if (GUILayout.Button("Create prefab") && AskCreatePrefab())
            {
                Debug.Log("Create Nested prefab");
                
                if(obstructionModel != null)
                    DestroyImmediate(obstructionModel);

                foreach (Transform child in prefabParent.transform)
                {
                    RecursivelyRemoveProceduralEntitiesChildren(child.gameObject);
                }

                string localPath = selectedNestedPrefabCategory.pathFolder + prefabParent.name + ".prefab";

                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
                
                PrefabUtility.SaveAsPrefabAssetAndConnect(prefabParent, localPath,
                    InteractionMode.UserAction);
                
                DestroyImmediate(prefabParent);
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
        if (!(exclusiveToggle[0] || exclusiveToggle[1] || exclusiveToggle[2]))
        {
            Debug.LogError("Please select one category");
            return false;
        }

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
        else
        {
            if (!Directory.Exists(selectedNestedPrefabCategory.pathFolder))
            {
                Directory.CreateDirectory(selectedNestedPrefabCategory.pathFolder);
            }
        }

        return true;
    }

    public void InstantiateObstructionModel()
    {
        if(obstructionModel != null)
            DestroyImmediate(obstructionModel);

        if (selectedNestedPrefabCategory.obstructionModel != null)
        {
            obstructionModel = Instantiate(selectedNestedPrefabCategory.obstructionModel);
            obstructionModel.transform.position = Vector3.zero;
        }
    }

    public void RecursivelyRemoveProceduralEntitiesChildren(GameObject parent)
    {
        DestroyImmediate(parent.GetComponent<ProceduralEntity>());
        
        foreach (Transform child in parent.transform)
        {
            RecursivelyRemoveProceduralEntitiesChildren(child.gameObject);
        }
    }
    
}
