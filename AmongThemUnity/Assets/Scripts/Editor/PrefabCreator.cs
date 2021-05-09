using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;


[ExecuteInEditMode]
public class PrefabCreator : EditorWindow
{
    private GameObject prefabParent;
    private List<GameObject> objectsInScene = new List<GameObject>();
    private int nbOldObjects = 1;
    private int nbChildren = 0;
    
    [MenuItem("Tools/Prefab Creator")]
    static void Init()
    {
        PrefabCreator window = (PrefabCreator)GetWindow(typeof(PrefabCreator));
        window.Show();
    }

    // Called two times when drag&drop a prefab :
    // first with selection = false, then with selection = true on the drop
    private void OnHierarchyChange()
    {
        Debug.Log("-------------------------------");

        var actualObjects = FindObjectsOfType(typeof(GameObject));
        var nbActualObjects = actualObjects.Length;
        Debug.Log(nbOldObjects + " " + nbActualObjects);
        
        if (Selection.activeTransform == null) // Change in hierarchy or prefab has been removed
        {
            if (nbActualObjects < nbOldObjects) // Prefab has been removed
            {
                Debug.Log("removed prefab");
                Selection.activeGameObject = prefabParent;
                if (prefabParent == null)
                {
                    Debug.Log("Prefab Parent has been removed.");
                    nbOldObjects = 1;
                }
                else
                {
                    nbOldObjects -= 1;
                }
            }
            else // Change in hierarchy
            {
                Debug.Log("Maybe change in hierachy");
                /*foreach (var acObj in actualObjects)
                {
                    GameObject CameOutChild = GameObjectCameOutParentPrefab((GameObject) acObj);
                    if(CameOutChild)
                }*/
            }
            
            
            return;
        }

        if (IsLightOfScene(Selection.activeGameObject) || Selection.activeGameObject == prefabParent)
        {
            Debug.Log("Light or parent");
            return;
        }
            
        Debug.Log("Before maybe prefab added");
        
        if (nbActualObjects != nbOldObjects) // Prefab added
        {
            Debug.Log("Maybe prefab added");
            if (nbOldObjects == 1 && nbActualObjects > 1) // prefabParentAdded
            {
                PrefabParentAdded(Selection.activeTransform.gameObject);
                nbOldObjects = nbActualObjects;
            }
            else if(nbActualObjects > nbOldObjects && !objectsInScene.Contains(Selection.activeGameObject)) // prefabChildAdded
            {
                Debug.Log("Add child prefab");
                Selection.activeTransform.parent = prefabParent.transform;
                Selection.activeTransform.position = Vector3.zero;
                AddChildRecursively(Selection.activeGameObject);
                nbOldObjects = nbActualObjects;
            }
        }
    }

    private void PrefabParentAdded(GameObject activeGameObject)
    {
        Light light;

        if (activeGameObject != null && !IsLightOfScene(activeGameObject))
        {
            Debug.Log("Create parent prefab");
            prefabParent = activeGameObject;
            prefabParent.transform.position = Vector3.zero;
            AddChildRecursively(prefabParent);
        }
    }

    

    private GameObject GameObjectCameOutParentPrefab(GameObject gameObject)
    {
        return GetParentGameObject(gameObject) != prefabParent ? gameObject : null;
    }

    
    private void AddChildRecursively(GameObject child)
    {
        objectsInScene.Add(child);
        
        foreach (Transform childSquare in child.transform)
        {
            AddChildRecursively(childSquare.gameObject);
        }
    }
    
    private void RemoveChildRecursively(GameObject child)
    {
        objectsInScene.Remove(child);
        
        foreach (Transform childSquare in child.transform)
        {
            RemoveChildRecursively(childSquare.gameObject);
        }
    }

    private bool IsLightOfScene(GameObject gameObject)
    {
        Light light;
        return gameObject.TryGetComponent(out light);
    }
    
    private GameObject GetParentGameObject(GameObject gameObject)
    {
        Transform transformChild = gameObject.transform;
        while (transformChild.parent != null)
        {
            transformChild = transformChild.parent;
        }

        return transformChild.gameObject;
    }
}
