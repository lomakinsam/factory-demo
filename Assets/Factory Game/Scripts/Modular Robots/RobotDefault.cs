using UnityEngine;

namespace ModularRobot
{
    public class RobotDefault : IRobot
    {
        private DamageableModule hull;
        private DamageableModule chip;
        private DamageableModule sensor;
        private Chassis chassis;

        public GameObject gameObject { get; private set; }

        public ModuleType[] Modules => new ModuleType[] { ModuleType.Hull, ModuleType.Chip, ModuleType.Sensor, ModuleType.Chassis }; 

        public RobotDefault(DamageableModule hull,
                            DamageableModule chip,
                            DamageableModule sensor,
                            DamageableModule wheel,
                            AxlesGroup[] chassisLayout)
        {
            GameObject robot = new GameObject("Robot Default");
            this.gameObject = robot;

            robot.AddComponent<Rigidbody>();
            robot.AddComponent<BoxCollider>();

            this.hull = Object.Instantiate(hull, Vector3.zero, Quaternion.identity);
            this.hull.transform.SetParent(robot.transform, true);
            this.hull.name = "Hull";

            Renderer hullRenderer = this.hull.GetComponent<Renderer>();
            Bounds hullBounds = hullRenderer.localBounds;

            Vector3 sensorPosition = this.hull.transform.TransformPoint(new Vector3(hullBounds.max.x,
                                                                                    hullBounds.max.y,
                                                                                    hullBounds.center.z));

            Vector3 chipPosition = this.hull.transform.TransformPoint(new Vector3(hullBounds.center.x,
                                                                                  hullBounds.max.y,
                                                                                  hullBounds.center.z));

            this.sensor = Object.Instantiate(sensor, sensorPosition, Quaternion.identity);
            this.sensor.transform.SetParent(robot.transform, true);
            this.sensor.name = "Sensor";

            this.chip = Object.Instantiate(chip, chipPosition, Quaternion.identity);
            this.chip.transform.SetParent(robot.transform, true);
            this.chip.name = "Chip";

            this.chassis = new Chassis(wheel, chassisLayout, hullRenderer, robot.transform);
        }

        public DamageType? DamageStatus
        {
            get
            {
                DamageType? robotStatus = null;
                IDamageable[] modules = { hull, chip, sensor, chassis };

                for (int i = 0; i < modules.Length; i++)
                {
                    var moduleStatus = modules[i].DamageStatus;
                    robotStatus ??= moduleStatus;
                    robotStatus = moduleStatus > robotStatus ? moduleStatus : robotStatus;
                }

                return robotStatus;
            }
        }

        public DamageType? GetDamageStatus(ModuleType module)
        {
            switch (module)
            {
                case ModuleType.Hull:
                    return hull.DamageStatus;
                case ModuleType.Chip:
                    return chip.DamageStatus;
                case ModuleType.Sensor:
                    return sensor.DamageStatus;
                case ModuleType.Chassis:
                    return chassis.DamageStatus;

                default:
                    return null;
            }
        }

        public DamageType? Repair(ModuleType module)
        {
            switch (module)
            {
                case ModuleType.Hull:
                    return hull.Repair();
                case ModuleType.Chip:
                    return chip.Repair();
                case ModuleType.Sensor:
                    return sensor.Repair();
                case ModuleType.Chassis:
                    return chassis.Repair();

                default:
                    return null;
            }
        }

        public void SetDamage(ModuleType module, DamageType damage)
        {
            if (module == ModuleType.Hull)
                hull.SetDamage(damage);
            if (module == ModuleType.Chip)
                chip.SetDamage(damage);
            if (module == ModuleType.Sensor)
                sensor.SetDamage(damage);
            if (module == ModuleType.Chassis)
                chassis.SetDamage(damage);
        }
    }
}