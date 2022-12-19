namespace ModularRobot
{
    public interface IDamageable
    {
        DamageType? DamageStatus { get; }

        void SetDamage(DamageType damage);
        DamageType? Repair();
    }

    public enum DamageType
    {
        LightDamage,
        AverageDamage,
        CriticalDamage
    }
}
