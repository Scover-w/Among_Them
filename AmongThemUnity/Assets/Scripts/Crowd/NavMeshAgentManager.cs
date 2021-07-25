using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public struct NewDestination
{
    public float waitingTime;
    public NavMeshAgent agent;
    public int idAnim;
}

enum AgentType
{
    Crowd,
    Cop,
    Target
}

public class NavMeshAgentManager : MonoBehaviour
{
    public static NavMeshAgentManager Instance()
    {
        return _singleton;
    }

    private static NavMeshAgentManager _singleton;

    private int nombreAgent;

    [SerializeField] private GameObject prefabAgent;

    [SerializeField] private GameObject prefabCops;

    [SerializeField] private GameObject prefabTarget;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform containerCrowd;

    private Vector3 randomPosition;

    private List<NavMeshAgent> agentList;
    private List<NavMeshAgent> copsList;
    private List<Animator> animators;

    private GameObject ToKillAgent;

    int layerMask = ~(1 << 8);

    private bool hasBeenDeleted = false;

    private Coroutine agentManagerCo;
    private List<Coroutine> newDestinationsCo = new List<Coroutine>();
    
    private Animator tempAnimator;
    private Vector3 tempTargetPos;
    private NavMeshPath tempNavMeshPath;
    private NavMeshAgent tempNavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _singleton = this;
        Time.timeScale = 1f;
        agentList = new List<NavMeshAgent>();
        animators = new List<Animator>();
        copsList = new List<NavMeshAgent>();
    }

    private void Update()
    {
        GetCaughtByCops();
    }

    public void InstantiateCrowd()
    {
        ClearCrowd();
        
        nombreAgent = (int) (500f * ProgressionManager.GetWealthValue() + 100f);
        nombreAgent = 100;

        for (int i = 0; i < nombreAgent; i++)
        {
            InstantiateAgent(AgentType.Crowd);
        }
        
        var nombreCops = (int) (nombreAgent / 100) > 0 ? (int) (nombreAgent / 100) : 1;

        for (int i = 0; i < nombreCops; i++)
        {
            InstantiateAgent(AgentType.Cop);
        }

        InstantiateAgent(AgentType.Target);

        agentManagerCo = StartCoroutine(nameof(ManageAgents));
    }

    private void ClearCrowd()
    {
        hasBeenDeleted = true;

        if (agentManagerCo != null)
            StopCoroutine(agentManagerCo);

        if (newDestinationsCo.Count > 0)
        {
            foreach (var co in newDestinationsCo)
            {
                StopCoroutine(co);
            }
        }

        newDestinationsCo = new List<Coroutine>();

        if (agentList.Count > 0)
        {
            foreach (var agent in agentList)
            {
                Destroy(agent.gameObject);
            }

            agentList = new List<NavMeshAgent>();
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

    }
    
    private void InstantiateAgent(AgentType type)
    {
        GameObject prefabToInst;

        switch (type)
        {
            case AgentType.Crowd:
                prefabToInst = prefabAgent;
                break;
            case AgentType.Cop:
                prefabToInst = prefabCops;
                break;
            case AgentType.Target:
                prefabToInst = prefabTarget;
                break;
            default:
                prefabToInst = prefabAgent;
                break;
        }
        
        var agent = Instantiate(prefabToInst, containerCrowd);
        tempNavMeshAgent = agent.GetComponent<NavMeshAgent>();

        Vector3 position;
        do
        {
            position = GetRandomPositionOnNavMesh();
        } while (position.y > 2f && position.y < 6.3f);

        tempNavMeshAgent.Warp(position);
        tempNavMeshPath = new NavMeshPath();
        tempTargetPos = GetRandomPositionOnNavMesh();
        tempNavMeshAgent.CalculatePath(tempTargetPos, tempNavMeshPath);
        tempNavMeshAgent.SetPath(tempNavMeshPath);
        tempAnimator = tempNavMeshAgent.gameObject.GetComponent<Animator>();
        animators.Add(tempAnimator);
        tempAnimator.SetBool("isWalking", true);
        agentList.Add(tempNavMeshAgent);
        
        if(type == AgentType.Cop)
            copsList.Add(tempNavMeshAgent);

        if (type == AgentType.Target)
            ToKillAgent = agent;
    }

    public IEnumerator ChangeDestinationAfterEvents(List<NavMeshAgent> agentsAffected, float waitingTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitingTime);
            foreach (var navMeshAgent in agentsAffected)
            {
                var targetPos = GetRandomPositionOnNavMesh();
                NavMeshPath navMeshPath = new NavMeshPath();
                navMeshAgent.CalculatePath(targetPos, navMeshPath);
                navMeshAgent.SetPath(navMeshPath);
            }
        }
    }

    public Vector3 GetRandomPositionOnNavMesh()
    {
        int areaPosition = Random.Range(0, 100);

        if (areaPosition < 10)
            randomPosition = new Vector3(Random.Range(-100, -94), Random.Range(0, 15), Random.Range(-70, 70));
        else if (areaPosition < 20)
            randomPosition = new Vector3(Random.Range(100, 94), Random.Range(0, 15), Random.Range(-70, 70));
        else if (areaPosition < 30)
            randomPosition = new Vector3(Random.Range(-100, 100), Random.Range(0, 15), Random.Range(-70, -64));
        else if (areaPosition < 40)
            randomPosition = new Vector3(Random.Range(-100, 100), Random.Range(0, 15), Random.Range(70, 64));
        else
            randomPosition = new Vector3(Random.Range(-100, 100), Random.Range(0, 15), Random.Range(-70, 70));

        //randomPosition = Random.insideUnitSphere * 250;

        return GetPositionOnNavMesh(randomPosition);
    }

    public Vector3 GetPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit = new NavMeshHit();

        NavMesh.SamplePosition(position, out hit, 50f, NavMesh.AllAreas);

        // try
        {
            return hit.position;
        }
        // catch (Exception e)
        // {
        //     // 
        // }

        return Vector3.zero;
    }

    public bool IsSomeoneWatching()
    {
        PlayerDetection playerDetection = player.GetComponent<PlayerDetection>();
        return playerDetection.IsPlayerVisible();
    }

    public List<NavMeshAgent> GetCrowdAgent()
    {
        return agentList;
    }

    public GameObject GetTargetAgent()
    {
        return ToKillAgent;
    }

    public void CopsGoOnCrimeScene()
    {
        foreach (var cops in copsList)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            cops.CalculatePath(ToKillAgent.transform.position, navMeshPath);
            cops.SetPath(navMeshPath);
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
        foreach (var agent in agentList)
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
            
            NavMeshPath navMeshPath = new NavMeshPath();
            agent.CalculatePath(location, navMeshPath);
            agent.SetPath(navMeshPath);
        }
    }


    IEnumerator ManageAgents()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wait;
            int i = 0;
            foreach (var agent in agentList)
            {
                if ((agent.transform.position - agent.destination).magnitude < 0.1f)
                {
                    animators[i].SetBool("isWalking", false);
                    float randomFloat = Random.Range(0f, 7f);
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
            NavMeshPath navMeshPath = new NavMeshPath();
            var targetPos = GetRandomPositionOnNavMesh();
            newDestination.agent.CalculatePath(targetPos, navMeshPath);
            newDestination.agent.SetPath(navMeshPath);
            animators[newDestination.idAnim].SetBool("isWalking", true);
        }
    }
}