using UnityEngine;
using UnityEngine.AI;

namespace TrailsPack
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Trail : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private TrailRenderer trailRenderer;

        public void Draw(Vector3 start, Vector3 target)
        {
            transform.position = start;

            agent.ResetPath();
            agent.SetDestination(new(target.x, trailRenderer.transform.position.y, target.z));

            trailRenderer.Clear();
        }
    }
}