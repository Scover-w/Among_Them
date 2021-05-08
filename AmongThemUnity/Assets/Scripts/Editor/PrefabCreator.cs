using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabCreator : UnityEditor.Editor
{
    private static string prefabPath = $@"\Prefabs\ProceduralEntities\";
    private static string blenderObjectPath = $@"\BlenderImport\";
    
    [MenuItem("Prefabs/Create")]
    public static void CreatePrefab()
    {
        RecursivePrefabCreator("Assets" + blenderObjectPath);
    }


    static void RecursivePrefabCreator(string path)
    {
        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        foreach (var file in files)
        {
            if (file.Contains(".meta"))
                continue;
            
            var prefabFile = file.Replace(blenderObjectPath, prefabPath).Replace(".fbx", ".prefab");
            if (!File.Exists(prefabFile))
            {
                var objectToPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
                objectToPrefab = Instantiate(objectToPrefab);
                objectToPrefab.AddComponent<ProceduralEntity>();
                objectToPrefab.transform.position = Vector3.zero;

                PrefabUtility.SaveAsPrefabAssetAndConnect(objectToPrefab, prefabFile, InteractionMode.UserAction);
                DestroyImmediate(objectToPrefab);
                Debug.Log(prefabFile + " created. (prefab)");
            }
        }

        foreach (var directory in directories)
        {
            string prefabDirectory = directory.Replace(blenderObjectPath, prefabPath);
            if (!Directory.Exists(prefabDirectory))
            {
                Debug.Log(prefabDirectory + " created. (directory)");
                Directory.CreateDirectory(prefabDirectory);
            }
                
            RecursivePrefabCreator(directory);
        }
        
    }
    
    // https://docs.unity3d.com/ScriptReference/EditorUtility.html
    
    // https://docs.unity3d.com/ScriptReference/PrefabUtility.html
    
    // https://docs.unity3d.com/ScriptReference/AssetDatabase.html
    
}
