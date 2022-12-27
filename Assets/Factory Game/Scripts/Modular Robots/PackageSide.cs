using UnityEngine;
using System.Collections;

namespace ModularRobot
{
    public class PackageSide : MonoBehaviour
    {
        private const float mass = 0.01f;
        private const float detachForce = 0.01f;
        private const float heightEvaluationOffset = 0.01f;
        private const float topSidePushAngle = 45f;
        private const float topSideForceMultiplayer = 3f;

        public void Detach(Vector3 packageCenter, float lifeTime)
        {
            transform.parent = null;

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = mass;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            float force = detachForce;
            Vector3 forceDir = transform.forward;

            if (transform.position.y > packageCenter.y + heightEvaluationOffset)
            {
                force *= topSideForceMultiplayer;
                forceDir = Quaternion.Euler(topSidePushAngle, 0f, 0f) * forceDir;
            }
                
            if (transform.position.y < packageCenter.y - heightEvaluationOffset)
                force = 0f;

            rb.AddForce(forceDir * force, ForceMode.Impulse);

            StartCoroutine(DestroyDelayed(lifeTime));
        }

        private IEnumerator DestroyDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(gameObject);
        }
    }
}