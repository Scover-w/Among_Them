using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

public class BlenderImportMaterials : Editor
{
    private static Material _mat;
    private static string _blenderObjectPath = $@"\Prefabs\ProceduralEntities\";
    
    [MenuItem("Tools/BlenderImport/AddGlobalMaterial")]
    public static void AddGlobalMaterial()
    {
        if(_mat == null)
            _mat = (Material)AssetDatabase.LoadAssetAtPath(@"Assets\Materials\White.mat", typeof(Material));

        RecursiveAddGlobalMaterial("Assets" + _blenderObjectPath);
    }

    public static void RecursiveAddGlobalMaterial(string path)
    {
        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        foreach (var file in files)
        {
            if (file.Contains(".meta"))
                continue;
            
            
            /*var blenderGameObject = (GameObject)AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));

            Debug.Log(blenderGameObject);
            blenderGameObject.GetComponent<Material>(). = _mat;*/

            return;
        }

        foreach (var directory in directories)
        {
            RecursiveAddGlobalMaterial(directory);
        }
    }
}
