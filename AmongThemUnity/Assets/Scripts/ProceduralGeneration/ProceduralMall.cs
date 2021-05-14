using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProceduralMall : MonoBehaviour
{
    public void LoadMall(List<ObstructedLocation> obstructedLocation, float wealthLevel)
    {
        var objects = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/NestedPrefabs/Mall/");

        List<GameObject> apartments = new List<GameObject>();
        
        var apartmentDict = new Dictionary<GameObject, float>();

        GameObject temp;
        foreach (var obj in objects)
        {
            temp = (GameObject) obj;
            apartmentDict.Add(temp, temp.GetComponent<ProceduralEntity>().wealthValue);
        }

        temp = Instantiate(ProceduralCalculations.GetRandomTFromPool(apartmentDict, wealthLevel));
        temp.transform.position = Vector3.zero;
    }
}
