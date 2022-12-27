using UnityEngine;

namespace Resources
{
    public class Supplies : MonoBehaviour, IPhysical
    {
        [SerializeField]
        private SuppliesType suppliesType;
        [SerializeField]
        private Rigidbody suppliesRigidbody;
        [SerializeField]
        private Collider suppliesCollider;

        public SuppliesType SuppliesType => suppliesType;

        public void EnablePhysics()
        {
            suppliesRigidbody.isKinematic = false;
            suppliesCollider.enabled = true;
        }

        public void DisablePhysics()
        {
            suppliesRigidbody.isKinematic = true;
            suppliesCollider.enabled = false;
        }
    }

    public enum SuppliesType { Pink, Blue, Yellow }
}