using UnityEngine;

namespace Resources
{
    public class Supplies : MonoBehaviour, IPhysical
    {
        [SerializeField]
        private SupplieType suppliesType;
        [SerializeField]
        private Rigidbody suppliesRigidbody;
        [SerializeField]
        private Collider suppliesCollider;

        public SupplieType SuppliesType => suppliesType;

        public void EnablePhysics()
        {
            suppliesRigidbody.isKinematic = false;
            suppliesRigidbody.detectCollisions = true;
            suppliesCollider.enabled = true;
        }

        public void DisablePhysics()
        {
            suppliesRigidbody.isKinematic = true;
            suppliesRigidbody.detectCollisions = false;
            suppliesCollider.enabled = false;
        }
    }

    public enum SupplieType { Pink, Blue, Yellow }
}