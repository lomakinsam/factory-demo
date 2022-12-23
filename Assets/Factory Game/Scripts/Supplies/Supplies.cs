using UnityEngine;

namespace Resources
{
    public class Supplies : MonoBehaviour
    {
        [SerializeField]
        private SuppliesType suppliesType;

        public SuppliesType SuppliesType => suppliesType;
    }

    public enum SuppliesType { Pink, Blue, Yellow }
}