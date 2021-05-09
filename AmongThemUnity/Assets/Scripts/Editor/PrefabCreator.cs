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
        Debug.Log("isPrefabparent setted ? : " + (prefabParent != null));

        var actualObjects = FindObjectsOfType(typeof(GameObject));
        var nbActualObjects = actualObjects.Length;

        
        if (Selection.activeTransform == null) // Change in hierarchy or prefab has been removed
        {
            if (nbActualObjects < nbOldObjects) // Prefab has been removed
            {
                Debug.Log("removed prefab");

                if (prefabParent == null)
                {
                    Debug.Log("Prefab Parent has been removed.");
                    nbOldObjects = 1;
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

        
        if (nbActualObjects != nbOldObjects) // Prefab added
        {
            if (nbOldObjects == 1 && nbActualObjects > 1) // prefabParentAdded
            {
                PrefabParentAdded(Selection.activeTransform.gameObject);
            }
            else if(nbActualObjects > nbOldObjects) // prefabChildAdded
            {
                SetParent(prefabParent.transform, Selection.activeTransform);
            }
            nbOldObjects = nbActualObjects;
        }
    }

    private void PrefabParentAdded(GameObject activeGameObject)
    {
        Light light;

        if (activeGameObject != null && !IsLightOfScene(activeGameObject))
        {
            prefabParent = activeGameObject;
            prefabParent.transform.position = Vector3.zero;
        }
    }

    

    private GameObject GameObjectCameOutParentPrefab(GameObject gameObject)
    {
        return GetParentGameObject(gameObject) != prefabParent ? gameObject : null;
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

    private void SetParent(Transform parent, Transform children)
    {
        children.SetParent(parent);
        children.position = Vector3.zero;
    }

}
