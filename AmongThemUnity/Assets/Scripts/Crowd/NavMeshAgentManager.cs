using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NavMeshAgentManager : MonoBehaviour
{
    public static NavMeshAgentManager Instance() { return _singleton; }
    private static NavMeshAgentManager _singleton;
    
    [SerializeField] 
    private int nombreAgent = 1;

    [SerializeField] 
    private GameObject prefabAgent;
    
    [SerializeField] 
    private GameObject toKillAgent;
    
    [SerializeField] 
    private GameObject player;

    private Vector3 randomPosition;

    private List<NavMeshAgent> navMeshList;
    private List<MeshCollider> fieldViewMeshColliderList;
    private List<Transform> fieldViewPositionList;

    private GameObject ToKillAgent;
    
    int layerMask = ~(1 << 8);
    
    // Start is called before the first frame update
    void Start()
    {
        _singleton = this;
        Time.timeScale = 1f;
        navMeshList = new List<NavMeshAgent>();
        fieldViewMeshColliderList = new List<MeshCollider>();
        fieldViewPositionList = new List<Transform>();
        
        for (int i = 0; i < nombreAgent; i++)
        {
            var agent = Instantiate(prefabAgent);
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(GetRandomPositionOnNavMesh());
            navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
            navMeshList.Add(navMeshAgent);
            fieldViewMeshColliderList.Add(agent.GetComponentInChildren<MeshCollider>());
            fieldViewPositionList.Add(agent.transform.GetChild(2));
        }
        
        ToKillAgent = Instantiate(toKillAgent);
        NavMeshAgent navMeshAgent2 = ToKillAgent.GetComponent<NavMeshAgent>();
        navMeshAgent2.Warp(GetRandomPositionOnNavMesh());
        navMeshAgent2.SetDestination(GetRandomPositionOnNavMesh());
        navMeshList.Add(navMeshAgent2);

        StartCoroutine(CheckNavMeshArrived());
    }

    IEnumerator CheckNavMeshArrived()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            foreach (var navMeshAgent in navMeshList)
            {
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
                        }
                    }
                } 
            } 
        }
    }

    public IEnumerator ChangeDestinationAfterEvents(List<NavMeshAgent> agentsAffected)
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            foreach (var navMeshAgent in agentsAffected)
            {
                navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
            }
        }
    }

    public Vector3 GetRandomPositionOnNavMesh()
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

    public bool IsSomeoneWatching()
    {
        PlayerDetection playerDetection = player.GetComponent<PlayerDetection>();
        return playerDetection.IsPlayerVisible();

        /*
        List<int> indexList = new List<int>();
        int i = 0;
        foreach (var fieldViewMeshCollider in fieldViewMeshColliderList)
        {
            if (fieldViewMeshCollider.bounds.Contains(ToKillAgent.transform.position))
            {
                Ray ray = new Ray(fieldViewPositionList[i].position, player.transform.position - fieldViewPositionList[i].position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 200f, layerMask))
                {
                    if (hit.transform.gameObject.tag.Contains("Player") || hit.transform.gameObject.tag.Contains("ToKillAgent"))
                    {
                        return true;
                    }
                }
            }

            i++;
        }
        return false;
        */
    }

    public void POICreated(Vector3 origin)
    {
        
    }

    public List<NavMeshAgent> GetCrowdAgent()
    {
        return navMeshList;
    }
}
