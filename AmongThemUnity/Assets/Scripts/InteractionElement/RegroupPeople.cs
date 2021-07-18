using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InteractionElement
{
    public class RegroupPeople : MonoBehaviour, EventScriptInterface
    {
        private Transform objectTransform;
        private List<NavMeshAgent> crowdAgentsList;
        [SerializeField]
        private int radius;
        [SerializeField]
        private float waitingTime;
    
        // Start is called before the first frame update
        void Start()
        {
            objectTransform = this.gameObject.transform;
            crowdAgentsList = NavMeshAgentManager.Instance().GetCrowdAgent();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                List<NavMeshAgent> agentsAffected = new List<NavMeshAgent>();

                foreach (var ca in crowdAgentsList)
                {
                    float distanceSqr = (objectTransform.position - ca.gameObject.transform.position).sqrMagnitude;
                    if (distanceSqr < radius)
                    {
                        agentsAffected.Add(ca);
                        NavMeshHit hit = new NavMeshHit();

                        NavMesh.SamplePosition(objectTransform.position, out hit, 16f, NavMesh.AllAreas);
                        ca.SetDestination(hit.position);
                    }
                }

                StartCoroutine(NavMeshAgentManager.Instance().ChangeDestinationAfterEvents(agentsAffected, waitingTime));
            }

            if (other.gameObject.tag.Equals("CrowdAgent"))
            {
                other.gameObject.GetComponent<NavMeshAgent>()
                    .SetDestination(NavMeshAgentManager.Instance().GetRandomPositionOnNavMesh());
            }
        }
    }
}
