using UnityEngine;
using System.Reflection;

namespace ModularRobot
{
    [CreateAssetMenu(menuName = "Robot Factory")]
    public class RobotFactory : ScriptableObject
    {
        [SerializeField]
        private RobotSimplified robotSimplified;

        [SerializeField]
        private DamageableModule[] hulls;
        [SerializeField]
        private DamageableModule[] chips;
        [SerializeField]
        private DamageableModule[] sensors;
        [SerializeField]
        private DamageableModule[] wheels;

        private DamageType randomDamage => (DamageType)Random.Range(0, System.Enum.GetValues(typeof(DamageType)).Length);

        public IRobot CreateRobotSimplified(bool allowDamage = false)
        {
            IRobot robot = Instantiate(robotSimplified, null);

            if (allowDamage)
            {
                bool isDamaged = false;

                foreach (var module in robot.Modules)
                {
                    int damageStatus = Random.Range(0, 2);

                    if (damageStatus == 1)
                    {
                        robot.SetDamage(module, randomDamage);
                        isDamaged = true;
                    }
                }

                if (!isDamaged)
                    robot.SetDamage(ModuleType.Hull, randomDamage);
            }

            return robot;
        }

        public IRobot CreateRobotDefault()
        {
            IRobot robot = new RobotDefault(hulls[0], chips[0], sensors[0], wheels[0], ChassisLayout.TwoAxles) as IRobot;
            return robot;
        }

        public IRobot CreateRobotRandom(bool allowDamage = false)
        {
            DamageableModule hull = hulls[Random.Range(0, hulls.Length)];
            DamageableModule chip = chips[Random.Range(0, chips.Length)];
            DamageableModule wheel = wheels[Random.Range(0, wheels.Length)];
            DamageableModule sensor = sensors[Random.Range(0, sensors.Length)];

            IRobot robot = new RobotDefault(hull, chip, sensor, wheel, GenerateRandomChassisLayout()) as IRobot;

            if (allowDamage)
            {
                foreach (var module in robot.Modules)
                {
                    int damageStatus = Random.Range(0, 2);

                    if (damageStatus == 1)
                        robot.SetDamage(module, randomDamage);
                }
            }

            return robot;
        }

        private AxlesGroup[] GenerateRandomChassisLayout()
        {
            var layoutTypes = typeof(ChassisLayout).GetProperties(BindingFlags.Public | BindingFlags.Static);
            return (AxlesGroup[])layoutTypes[Random.Range(0, layoutTypes.Length)].GetValue(null);
        }
    }
}