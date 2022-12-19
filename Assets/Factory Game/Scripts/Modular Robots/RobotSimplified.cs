using UnityEngine;

namespace ModularRobot
{
    public class RobotSimplified : MonoBehaviour, IRobot
    {
        [SerializeField]
        private DamageableModule hull;
        [SerializeField]
        private DamageableModule chassis;
        [SerializeField]
        private Rigidbody robotRigidbody;

        public ModuleType[] Modules => new ModuleType[] { ModuleType.Hull, ModuleType.Chassis };

        public Rigidbody Rigidbody => robotRigidbody; 
            

        public DamageType? DamageStatus
        {
            get
            {
                DamageType? robotStatus = null;
                IDamageable[] modules = { hull, chassis };

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
            if (module == ModuleType.Chassis)
                chassis.SetDamage(damage);
        }
    }
}
