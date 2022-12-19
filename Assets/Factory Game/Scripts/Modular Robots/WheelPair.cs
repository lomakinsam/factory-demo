using UnityEngine;

namespace ModularRobot
{
    public class WheelPair : IDamageable
    {
        private DamageableModule wheelLeft;
        private DamageableModule wheelRight;

        public DamageType? DamageStatus
        {
            get
            {
                if (wheelLeft.DamageStatus == null && wheelRight.DamageStatus == null)
                    return null;
                if (wheelLeft.DamageStatus == null || wheelRight.DamageStatus == null)
                    return wheelLeft.DamageStatus ?? wheelRight.DamageStatus;
                
                if (wheelLeft.DamageStatus >= wheelRight.DamageStatus)
                    return wheelLeft.DamageStatus;
                else
                    return wheelRight.DamageStatus;  
            }
        }

        public WheelPair(DamageableModule wheelInstance,
                         Vector3 axleCenter,
                         float trackWidth,
                         Transform parent)
        {
            Quaternion leftWheelRotation = parent.rotation;
            Quaternion rightWheelRotation = leftWheelRotation * Quaternion.AngleAxis(180.0f, Vector3.right);

            Vector3 wheelExtents = wheelInstance.GetComponent<Renderer>().bounds.extents;
            Vector3 leftWheelOffset = parent.TransformDirection(new Vector3(0, 0, -(trackWidth / 2) - wheelExtents.z));
            Vector3 rightWheelOffset = parent.TransformDirection(new Vector3(0, 0, (trackWidth / 2) + wheelExtents.z));

            Vector3 leftWheelPosition = axleCenter + leftWheelOffset;
            Vector3 rightWheelPosition = axleCenter + rightWheelOffset;

            GameObject wheelPair = new GameObject($"Wheel Pair ({parent.childCount})");
            wheelPair.transform.SetParent(parent, true);

            wheelLeft = Object.Instantiate(wheelInstance, leftWheelPosition, leftWheelRotation);
            wheelLeft.transform.SetParent(wheelPair.transform, true);
            wheelLeft.name = "Left wheel";

            wheelRight = Object.Instantiate(wheelInstance, rightWheelPosition, rightWheelRotation);
            wheelRight.transform.SetParent(wheelPair.transform, true);
            wheelRight.name = "Right wheel";
        }

        public DamageType? Repair()
        {
            wheelLeft.Repair();
            wheelRight.Repair();
            return DamageStatus;
        }

        public void SetDamage(DamageType damage)
        {
            wheelLeft.SetDamage(damage);
            wheelRight.SetDamage(damage);
        }
    }
}