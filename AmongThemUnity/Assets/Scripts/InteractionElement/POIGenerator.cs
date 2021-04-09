using System.Collections;
using System.Collections.Generic;
using InteractionElement;
using UnityEngine;
using UnityEngine.AI;
public class POIGenerator : MonoBehaviour
{
    [SerializeField] 
    private int nbPOI = 20;

    [SerializeField] 
    private GameObject prefabPOI;
    
    private GameObject poi;
    
    private Vector3 randomPosition;
    
    public List<EventScriptInterface> eventScriptList = new List<EventScriptInterface>();
    void Start()
    {
        for (int i = 0; i < nbPOI; i++)
        {
            poi = Instantiate(prefabPOI);
            poi.transform.position = GetRandomPositionOnNavMesh();
            poi.transform.position += Vector3.up * 1.8f;
        }
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        int areaPosition = Random.Range(0, 100);
        
        if(areaPosition < 10)
            randomPosition = new Vector3(Random.Range(-100,-94),Random.Range(0,15),Random.Range(-70,70));
        else if (areaPosition < 20)
            randomPosition = new Vector3(Random.Range(100, 94),Random.Range(0,15),Random.Range(-70,70));
        else if (areaPosition < 30)
            randomPosition = new Vector3(Random.Range(-100,100),Random.Range(0,15),Random.Range(-70,-64));
        else if (areaPosition < 40)
            randomPosition = new Vector3(Random.Range(-100,100),Random.Range(0,15),Random.Range(70,64));
        else
            randomPosition = new Vector3(Random.Range(-100,100),Random.Range(0,15),Random.Range(-70,70));
        
        NavMeshHit hit = new NavMeshHit();

        NavMesh.SamplePosition(randomPosition, out hit, 16f, NavMesh.AllAreas);
        return hit.position;
    }
}
