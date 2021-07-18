using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public struct NewDestination
{
    public float waitingTime;
    public NavMeshAgent agent;
    public int idAnim;
}

public class NavMeshAgentManager : MonoBehaviour
{
    public static NavMeshAgentManager Instance() { return _singleton; }
    private static NavMeshAgentManager _singleton;
    
    private int nombreAgent;

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
    private List<Animator> animators;
    private List<MeshCollider> fieldViewMeshColliderList;
    private List<Transform> fieldViewPositionList;

    private GameObject ToKillAgent;
    
    int layerMask = ~(1 << 8);

    private bool hasBeenDeleted = false;

    private Coroutine agentManagerCo;
    private List<Coroutine> newDestinationsCo = new List<Coroutine>();
    
    // Start is called before the first frame update
    void Start()
    {
        _singleton = this;
        Time.timeScale = 1f;
        navMeshList = new List<NavMeshAgent>();
        animators = new List<Animator>();
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

        hasBeenDeleted = true;
        
        if(agentManagerCo != null)
            StopCoroutine(agentManagerCo);

        if (newDestinationsCo.Count > 0)
        {
            foreach (var co in newDestinationsCo)
            {
                StopCoroutine(co);
            }
        }
        
        newDestinationsCo = new List<Coroutine>();
        
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
        
        animators = new List<Animator>();

        nombreAgent = (int)(500f * ProgressionManager.GetWealthValue() + 100f);

        Animator animator;
        
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
            animator = navMeshAgent.gameObject.GetComponent<Animator>();
            animators.Add(animator);
            animator.SetBool("isWalking", true);
            navMeshList.Add(navMeshAgent);

            fieldViewMeshColliderList.Add(agent.GetComponentInChildren<MeshCollider>());
            fieldViewPositionList.Add(agent.transform.GetChild(0));
        }
        
        ToKillAgent = Instantiate(toKillAgent, containerCrowd);
        NavMeshAgent navMeshAgent2 = ToKillAgent.GetComponent<NavMeshAgent>();
        
        Vector3 position2;
        do
        {
            position2 = GetRandomPositionOnNavMesh();
        } while (position2.y > 2f && position2.y < 6.3f);
        navMeshAgent2.Warp(position2);
        
        navMeshAgent2.SetDestination(GetRandomPositionOnNavMesh());
        navMeshList.Add(navMeshAgent2);
        
        animator = navMeshAgent2.gameObject.GetComponent<Animator>();
        animators.Add(animator);
        animator.SetBool("isWalking", true);
        
        var nombreCops = (int) (nombreAgent / 100) > 0 ? (int) (nombreAgent / 100) : 1;
        
        for (int i = 0; i < nombreCops; i++)
        {
            var cops = Instantiate(prefabCops, containerCrowd);
            NavMeshAgent navMeshAgent = cops.GetComponent<NavMeshAgent>();
            
            Vector3 position;
            do
            {
                position = GetRandomPositionOnNavMesh();
            } while (position.y > 2f && position.y < 6.3f);
            
            navMeshAgent.Warp(position);
            navMeshAgent.SetDestination(GetRandomPositionOnNavMesh());
            navMeshList.Add(navMeshAgent);
            animator = navMeshAgent.gameObject.GetComponent<Animator>();
            animators.Add(animator);
            animator.SetBool("isWalking", true);
            copsList.Add(navMeshAgent);
            fieldViewMeshColliderList.Add(cops.GetComponentInChildren<MeshCollider>());
            fieldViewPositionList.Add(cops.transform.GetChild(0));
        }

        agentManagerCo = StartCoroutine(nameof(ManageAgents));
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
        
        foreach (var agent in navMeshList)
        {
            RegroupAround(agent, position);
        }

        foreach (var agent in copsList)
        {
            RegroupAround(agent, position);
        }
    }

    private void RegroupAround(NavMeshAgent agent, Vector3 position)
    {
        Vector3 location;
        float distance = (agent.gameObject.transform.position - position).magnitude;
 
        if (distance < 15f)
        {
            location = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)) + position;
                
            location = GetPositionOnNavMesh(location);

            if (location == Vector3.zero)
                return;

            agent.SetDestination(location);
        }
    }


    IEnumerator ManageAgents()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wait;
            int i = 0;
            foreach(var agent in navMeshList) 
            {
                if ((agent.transform.position - agent.destination).magnitude < 0.1f)
                {
                    animators[i].SetBool("isWalking", false);
                    float randomFloat = Random.Range(0, 5);
                    NewDestination newDestination = new NewDestination();
                    newDestination.waitingTime = randomFloat;
                    newDestination.agent = agent;
                    newDestination.idAnim = i;
                    hasBeenDeleted = false;
                    newDestinationsCo.Add(StartCoroutine(nameof(SetNewDestination), newDestination));
                }

                i++;
            }
        }
    }

    IEnumerator SetNewDestination(NewDestination newDestination)
    {
        yield return new WaitForSeconds(newDestination.waitingTime);
        if (!hasBeenDeleted)
        {
            newDestination.agent.SetDestination(GetRandomPositionOnNavMesh());
            animators[newDestination.idAnim].SetBool("isWalking", true);
        }
    }
}
