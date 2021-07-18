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
    private int nombreAgent = 50;

    [SerializeField] 
    private GameObject prefabAgent;
    
    [SerializeField] 
    private GameObject prefabCops;
    
    [SerializeField] 
    private GameObject toKillAgent;
    
    [SerializeField] 
    private GameObject player;

    [SerializeField] 
    private Transform containerCrowd;

    private Vector3 randomPosition;

    private List<NavMeshAgent> navMeshList;
    private List<NavMeshAgent> copsList;
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
        copsList = new List<NavMeshAgent>();
        fieldViewMeshColliderList = new List<MeshCollider>();
        fieldViewPositionList = new List<Transform>();


    }
    
    private void Update()
    {
        GetCaughtByCops();
    }

    public void InstantiateCrowd()
    {
        if (navMeshList.Count > 0)
        {
            foreach (var agent in navMeshList)
            {
                Destroy(agent.gameObject);
            }
            navMeshList = new List<NavMeshAgent>();
        }
        
        if (copsList.Count > 0)
        {
            foreach (var agent in copsList)
            {
                Destroy(agent.gameObject);
            }
            copsList = new List<NavMeshAgent>();
        }
        
        for (int i = 0; i < nombreAgent; i++)
        {
            var agent = Instantiate(prefabAgent, containerCrowd);
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();

            Vector3 position;
            do
            {
                position = GetRandomPositionOnNavMesh();
            } while (position.y > 2f && position.y < 6.3f);
            
            navMeshAgent.Warp(position);
            navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
            navMeshList.Add(navMeshAgent);
            fieldViewMeshColliderList.Add(agent.GetComponentInChildren<MeshCollider>());
            fieldViewPositionList.Add(agent.transform.GetChild(0));
        }
        
        ToKillAgent = Instantiate(toKillAgent, containerCrowd);
        NavMeshAgent navMeshAgent2 = ToKillAgent.GetComponent<NavMeshAgent>();
        navMeshAgent2.Warp(GetRandomPositionOnNavMesh());
        navMeshAgent2.SetDestination(GetRandomPositionOnNavMesh());
        navMeshList.Add(navMeshAgent2);
        
        var nombreCops = (int) (nombreAgent / 100) > 0 ? (int) (nombreAgent / 100) : 1;
        
        for (int i = 0; i < nombreCops; i++)
        {
            var cops = Instantiate(prefabCops, containerCrowd);
            NavMeshAgent navMeshAgent = cops.GetComponent<NavMeshAgent>();
            
            
            navMeshAgent.Warp(GetRandomPositionOnNavMesh());
            navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
            navMeshList.Add(navMeshAgent);
            copsList.Add(navMeshAgent);
            fieldViewMeshColliderList.Add(cops.GetComponentInChildren<MeshCollider>());
            fieldViewPositionList.Add(cops.transform.GetChild(0));
        }
    }
    
    public IEnumerator ChangeDestinationAfterEvents(List<NavMeshAgent> agentsAffected, float waitingTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitingTime);
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

        //randomPosition = Random.insideUnitSphere * 250;

        return GetPositionOnNavMesh(randomPosition);
    }

    public Vector3 GetPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit = new NavMeshHit();

        NavMesh.SamplePosition(position, out hit, 50f, NavMesh.AllAreas);

        try
        {
            return hit.position;
        }
        catch (Exception e)
        {
            // 
        }
        
        return Vector3.zero;
    }

    public bool IsSomeoneWatching()
    {
        PlayerDetection playerDetection = player.GetComponent<PlayerDetection>();
        return playerDetection.IsPlayerVisible();
    }

    public List<NavMeshAgent> GetCrowdAgent()
    {
        return navMeshList;
    }

    public GameObject GetTargetAgent()
    {
        return ToKillAgent;
    }
    
    public void CopsGoOnCrimeScene()
    {
        foreach (var cops in copsList)
        {
            cops.SetDestination(ToKillAgent.transform.position);
            cops.speed *= 2;
        }
    }
    
    public bool GetCaughtByCops()
    {
        if (!GameManager.Instance().TargetIsAlive)
        {
            PlayerDetection playerDetection = player.GetComponent<PlayerDetection>();
            if (playerDetection.CopsWatchingYou())
            {
                GameManager.Instance().EndGame(false);
            }
        }

        return false;
    }

    public void RegroupAround(Vector3 position)
    {
        Vector3 location;
        foreach (var agent in navMeshList)
        {
            float distance = (agent.gameObject.transform.position - position).magnitude;
            Debug.Log(distance);
            if (distance < 15f)
            {
                location = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)) + position;
                
                location = GetPositionOnNavMesh(location);
                
                if(location == Vector3.zero)
                    continue;
                
                Debug.Log("Youpi");
                agent.Warp(location);
                agent.SetDestination(location);
            }
        }
    }
}
