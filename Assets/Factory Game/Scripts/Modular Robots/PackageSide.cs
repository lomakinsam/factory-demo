using UnityEngine;
using System.Collections;

namespace ModularRobot
{
    public class PackageSide : MonoBehaviour
    {
        private float pushForce = 1.5f;

        public void DestroyDelayed(float delay) => StartCoroutine(_DestroyDelayed(delay));

        public void EnablePhysics(bool pushSideways = false)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.01f;

            Vector3 pushDirection = pushSideways ? Quaternion.Euler(90f, 0f, 0f) * transform.forward : transform.forward;
            float forceMultiplier = pushSideways ? 3 : 1;

            rb.AddForce(pushDirection * pushForce * forceMultiplier, ForceMode.VelocityChange);
        }

        private IEnumerator _DestroyDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(gameObject);
        }
    }
}