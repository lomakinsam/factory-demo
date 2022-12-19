using UnityEngine;
using System.Collections.Generic;

namespace ModularRobot
{
    public class Chassis : IDamageable
    {
        private const float WHEEL_SPACING_MAGNITUDE = 0.2f;

        private AxlesGroup[] сhassisLayout;
        private List<WheelPair> wheelPairs;

        public Chassis(DamageableModule wheelInstance,
                       AxlesGroup[] chassisLayout,
                       Renderer relatedRenderer,
                       Transform parent)
        {
            this.сhassisLayout = chassisLayout;
            this.wheelPairs = new List<WheelPair>();

            Renderer wheelRenderer = wheelInstance.GetComponent<Renderer>();
            Bounds bounds = relatedRenderer.localBounds;

            float trackWidth = GetTrackWidth(relatedRenderer);
            float wheelDiameter = wheelRenderer.bounds.size.x;
            float wheelsSpacing = wheelDiameter * WHEEL_SPACING_MAGNITUDE;

            GameObject chassis = new GameObject("Chassis");
            chassis.transform.SetParent(parent, true);

            for (int i = 0; i < chassisLayout.Length; i++)
            {
                float wheelGroupLenght = (wheelDiameter * chassisLayout[i].AxlesNumber) +
                                         (wheelsSpacing * (chassisLayout[i].AxlesNumber - 1));

                float wheelGroupCenter_X = bounds.min.x + bounds.size.x * chassisLayout[i].Offset;
                float wheelGroupInitOffset_X = -wheelGroupLenght / 2 + wheelDiameter / 2;

                Vector3 wheelGroupCenter = relatedRenderer.transform.TransformPoint(new Vector3(wheelGroupCenter_X,
                                                                                                bounds.center.y,
                                                                                                bounds.center.z));

                Vector3 offset = relatedRenderer.transform.TransformDirection(new Vector3(wheelGroupInitOffset_X, 0, 0));
                Vector3 axleCenter = wheelGroupCenter + offset;

                offset = relatedRenderer.transform.TransformDirection(new Vector3(wheelDiameter + wheelsSpacing, 0, 0));

                for (int j = 0; j < chassisLayout[i].AxlesNumber; j++)
                {
                    wheelPairs.Add(new WheelPair(wheelInstance, axleCenter, trackWidth, chassis.transform));
                    axleCenter = axleCenter + offset;
                }
            }
        }

        public DamageType? DamageStatus
        {
            get
            {
                DamageType? chassisStatus = null;

                foreach (var wheelPair in wheelPairs)
                {
                    var wheelPairStatus = wheelPair.DamageStatus;
                    chassisStatus ??= wheelPairStatus;
                    chassisStatus = wheelPairStatus > chassisStatus ? wheelPairStatus : chassisStatus;
                }

                return chassisStatus;
            }
        }

        public DamageType? Repair()
        {
            DamageType? chassisStatus = null;

            foreach (var wheelPair in wheelPairs)
            {
                var wheelPairStatus = wheelPair.Repair();

                chassisStatus ??= wheelPairStatus;
                chassisStatus = wheelPairStatus > chassisStatus ? wheelPairStatus : chassisStatus;
            }

            return chassisStatus;
        }

        public void SetDamage(DamageType damage)
        {
            foreach (var wheelPair in wheelPairs)
            {
                wheelPair.SetDamage(damage);
            }
        }

        private float GetTrackWidth(Renderer renderer)
        {
            Vector3 axleMaxPoint = new Vector3(renderer.localBounds.center.x,
                                               renderer.localBounds.center.y,
                                               renderer.localBounds.center.z + renderer.localBounds.extents.z);

            Vector3 axleMinPoint = new Vector3(renderer.localBounds.center.x,
                                               renderer.localBounds.center.y,
                                               renderer.localBounds.center.z - renderer.localBounds.extents.z);

            float trackWidth = Vector3.Distance(renderer.transform.TransformPoint(axleMinPoint),
                                                renderer.transform.TransformPoint(axleMaxPoint));

            return trackWidth;
        }
    }
}
