using UnityEngine;

namespace ModularRobot
{
    public class DamageableModule : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Materials relatedMaterials;
        private Renderer m_renderer;

        public DamageType? DamageStatus { get; private set; }

        private void Awake()
        {
            DamageStatus = null;
            m_renderer = GetComponent<Renderer>();
        }

        public DamageType? Repair()
        {
            if (DamageStatus == DamageType.CriticalDamage)
            {
                m_renderer.material = relatedMaterials.averageDamage;
                DamageStatus = DamageType.AverageDamage;
                return DamageStatus;
            }

            if (DamageStatus == DamageType.AverageDamage)
            {
                m_renderer.material = relatedMaterials.lightDamage;
                DamageStatus = DamageType.LightDamage;
                return DamageStatus;
            }

            if (DamageStatus == DamageType.LightDamage)
            {
                m_renderer.material = relatedMaterials.noDamage;
                DamageStatus = null;
                return DamageStatus;
            }

            return DamageStatus;
        }

        public void SetDamage(DamageType damage)
        {
            if (damage == DamageStatus) return;

            if (damage == DamageType.CriticalDamage)
            {
                m_renderer.material = relatedMaterials.criticalDamage;
                DamageStatus = DamageType.CriticalDamage;
            }

            if (damage == DamageType.AverageDamage)
            {
                m_renderer.material = relatedMaterials.averageDamage;
                DamageStatus = DamageType.AverageDamage;
            }

            if (damage == DamageType.LightDamage)
            {
                m_renderer.material = relatedMaterials.lightDamage;
                DamageStatus = DamageType.LightDamage;
            }
        }

        [System.Serializable]
        private class Materials
        {
            public Material noDamage;
            public Material lightDamage;
            public Material averageDamage;
            public Material criticalDamage;
        }
    }
}