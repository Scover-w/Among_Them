using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAnimation : MonoBehaviour
{

    private Animator agentAnimator;
    private NavMeshAgent agentNavMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        agentAnimator = GetComponent<Animator>();
        agentNavMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agentNavMesh.isStopped)
        {
            Debug.Log("Stop");
            agentAnimator.SetBool("isWalking", false);
        }
        else
        {
            agentAnimator.SetBool("isWalking", true);
        }
    }
}
