using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProceduralMall : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] contentMall;
    public void LoadMall(List<ObstructedLocation> obstructedLocation, float wealthLevel)
    {
        var mallDict = new Dictionary<GameObject, float>();
        
        foreach (var obj in contentMall)
        {
            if(!(obstructedLocation.Contains(ObstructedLocation.Middle) && obj.GetComponent<ProceduralEntity>().obstructedLocation.Contains(ObstructedLocation.Middle)))
                mallDict.Add(obj, obj.GetComponent<ProceduralEntity>().wealthValue);
        }

        GameObject temp = Instantiate(ProceduralCalculations.GetRandomTFromPool(mallDict, wealthLevel));
        temp.transform.position = Vector3.zero;
    }
}
