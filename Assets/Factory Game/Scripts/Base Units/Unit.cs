using UnityEngine;
using UnityEngine.AI;

namespace BaseUnit
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        protected NavMeshAgent navMeshAgent;

        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void MoveTo(Vector3 position)
        {
            navMeshAgent.SetDestination(position);
        }
    }
}