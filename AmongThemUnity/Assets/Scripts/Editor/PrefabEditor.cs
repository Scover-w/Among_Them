using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabEditor : Editor
{
    private static string _prefabPath = $@"\Prefabs\";
    
    [MenuItem("Tools/Prefabs/AddMeshCollider")]
    public static void AddMeshCollider()
    {
        RecursiveAddMeshCollider("Assets" + _prefabPath);
    }

    public static void RecursiveAddMeshCollider(string path)
    {
        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        MeshCollider meshCollider;
        MeshCollider meshColliderChild;
        
        foreach (var file in files)
        {
            if (file.Contains(".meta"))
                continue;
            
            
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
            
            meshCollider = prefab.GetComponent<MeshCollider>();

            if (meshCollider == null)
                prefab.AddComponent<MeshCollider>();
            
            foreach (Transform child in prefab.transform)
            {
                meshColliderChild = child.GetComponent<MeshCollider>();
                
                if(meshColliderChild == null)
                    child.gameObject.AddComponent<MeshCollider>();
            }
        }

        foreach (var directory in directories)
        {
            RecursiveAddMeshCollider(directory);
        }
    }
}
