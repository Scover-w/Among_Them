using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{

    private Animator agentAnimator;
    private NavMeshAgent agentNavMesh;
    private Vector3 destination;
    private bool hasDestination;
    
    // Start is called before the first frame update
    void Start()
    {
        agentAnimator = GetComponent<Animator>();
        agentNavMesh = GetComponent<NavMeshAgent>();

        destination = agentNavMesh.destination;
        hasDestination = true;
        //StartCoroutine(CheckNavMeshArrived());
    }

    // Update is called once per frame
    void Update()
    {
        if (agentNavMesh.transform.position == destination)
        {
            agentNavMesh.GetComponent<Animator>().SetBool("isWalking", false);
            destination = Vector3.zero;
            hasDestination = false;
            float randomFloat = Random.Range(0, 5);
            StartCoroutine(GetANewDestination(randomFloat));
        }
    }

    IEnumerator GetANewDestination(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        destination = NavMeshAgentManager.Instance().GetRandomPositionOnNavMesh();
        hasDestination = true;
        agentNavMesh.SetDestination(destination);
        agentNavMesh.GetComponent<Animator>().SetBool("isWalking", true);
    }
}
