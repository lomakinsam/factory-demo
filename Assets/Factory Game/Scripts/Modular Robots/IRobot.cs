using UnityEngine;

namespace ModularRobot
{
    public interface IRobot
    {
        GameObject gameObject { get; }
        DamageType? DamageStatus { get; }

        DamageType? GetDamageStatus(ModuleType module);
        void SetDamage(ModuleType module, DamageType damage);
        DamageType? Repair(ModuleType module);
    }

    public enum ModuleType
    {
        Chassis,
        Chip,
        Hull,
        Sensor,
    }
}
