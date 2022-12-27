using UnityEngine;

namespace ModularRobot
{
    public class Package : MonoBehaviour, IPhysical
    {
        public IRobot WrappedItem { get; private set; }

        public Rigidbody Rigidbody { get; private set; }
        private const float mass = 2.5f;

        private PackageSide[] packageSides;
        private const float detachedSideLifeTime = 2.5f;
        
        private readonly Vector3 size = new (1f, 0.65f, 1.2f);
        private readonly Vector3 wrapOffset = new (0f, 0.2f, 0f);
        private readonly float thickness = 0.01f;

        private void Awake() => SetName();

        private void OnMouseDown() => Unwrap();

        public void EnablePhysics() => Rigidbody.isKinematic = false;

        public void DisablePhysics() => Rigidbody.isKinematic = true;

        public void Wrap(IRobot wrappedItem, PackageSide packageSide)
        {
            WrappedItem = wrappedItem;

            transform.position = WrappedItem.gameObject.transform.position + wrapOffset;
            WrappedItem.gameObject.transform.SetParent(transform, true);

            if (WrappedItem is RobotSimplified robotSimplified) robotSimplified.DisablePhysics();

            GameObject sidesContainer = new("Package Sides");
            sidesContainer.transform.SetParent(transform);
            sidesContainer.transform.localPosition = Vector3.zero;
            sidesContainer.transform.localRotation = Quaternion.identity;
            sidesContainer.transform.localScale = Vector3.one;

            Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.mass = mass;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            packageSides = new PackageSide[6];

            Vector3 sidePosition;
            Vector3 sideScale;
            Quaternion sideRotation;
            
            for (int i = 0; i < 6; i++)
            {
                packageSides[i] = Instantiate(packageSide);
                packageSides[i].gameObject.transform.SetParent(sidesContainer.transform);

                if (i < 4)
                {
                    float offset = i % 2 == 0 ? size.z / 2 : size.x / 2;
                    offset -= thickness / 2; 

                    float width = i % 2 == 0 ? size.x : size.z;
                    float height = size.y;
                    float rotationAngle = 90f * i;

                    sideRotation = Quaternion.Euler(0f, rotationAngle, 0f);

                    sidePosition = Vector3.zero + transform.forward * offset;
                    sidePosition = sideRotation * sidePosition;

                    sideScale = new(width, height, thickness);
                }
                else
                {
                    float offset = size.y / 2;
                    float width = size.x;
                    float height = size.z;
                    float rotationAngle = i % 2 == 0 ? 90f : -90f;

                    sideRotation = Quaternion.Euler(rotationAngle, 0f, 0f);

                    sidePosition = Vector3.zero + transform.forward * offset;
                    sidePosition = sideRotation * sidePosition;

                    sideScale = new(width, height, thickness);
                }

                packageSides[i].gameObject.transform.localPosition = sidePosition;
                packageSides[i].gameObject.transform.localRotation = sideRotation;
                packageSides[i].gameObject.transform.localScale = sideScale;
            }
        }

        public void Unwrap()
        {
            if (WrappedItem == null) return;

            if (WrappedItem is RobotSimplified robotSimplified) robotSimplified.EnablePhysics();

            WrappedItem.gameObject.transform.parent = null;
            WrappedItem = null;

            for (int i = 0; i < packageSides.Length; i++)
                packageSides[i].Detach(transform.position, detachedSideLifeTime);

            Destroy(gameObject);
        }

        private void SetName() => gameObject.name = $"Package ({gameObject.GetHashCode()})";
    }
}