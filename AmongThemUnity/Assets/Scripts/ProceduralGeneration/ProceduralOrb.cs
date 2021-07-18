using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class ProceduralOrb : MonoBehaviour
{
    [SerializeField] 
    private Transform containerMap;

    [SerializeField] 
    private GameObject mountOrb;

    [SerializeField] 
    private GameObject orb;
    
    public void LoadOrb(float wealthLevel)
    {
        int nbOrb = UnityEngine.Random.Range(0, 3);

        if (wealthLevel > 0.7f && nbOrb == 2)
            return;

        for (int i = 0; i < nbOrb; i++)
        {
            Load();
        }
    }

    private void Load()
    {
        int nbOrb = UnityEngine.Random.Range(0, 4);

        Vector3 location;
        
        
        // 70 / 100
        switch (nbOrb)
        {
            case 0:
                location = new Vector3(Rd(-100f, 100f), RdY(), Rd(70f, 85f));
                break;
            
            case 1:
                location = new Vector3(Rd(-100f, 100f), RdY(), Rd(-85f, -70f));
                break;
            
            case 2:
                location = new Vector3(Rd(100f, 125f), RdY(), Rd(-70f, 70f));
                break;
            
            default:
                location = new Vector3(Rd(-125f, -100f), RdY(), Rd(-70f, 70f));
                break;
        }

        NavMeshHit hit = new NavMeshHit();

        NavMesh.SamplePosition(location, out hit, 200f, NavMesh.AllAreas);

        GameObject mountOrbT = Instantiate(mountOrb, containerMap);
        mountOrbT.transform.position = hit.position;
        
        GameObject orbT = Instantiate(orb, containerMap);
        orbT.transform.position = hit.position + new Vector3(0f, .6f, 0f);
        orbT.transform.parent = mountOrbT.transform;
    }

    private float RdY()
    {
        if(Rd(0f, 1f) < .5f)
            return Rd(0f, 35f);
        return 0f;
    }
    private float Rd(float v1, float v2)
    {
        return UnityEngine.Random.Range(v1, v2);
    }
}
