using UnityEngine;
using System.Collections;

namespace ModularRobot
{
    public class RobotSpawner : MonoBehaviour
    {
        [SerializeField]
        private RobotFactory robotFactory;
        [SerializeField]
        private float initialDelay = 2f;
        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float spawnDelay;
        [SerializeField]
        private Vector2 forwardPushLimits = new Vector2(50f, 75f);
        [SerializeField]
        private Vector2 pushAngleLimits = new Vector2(-30f, 30f);
        [SerializeField]
        private PackageSide packageSide;

        private Coroutine spawnCycle;

        private void Start()
        {
            spawnCycle = StartCoroutine(SpawnRobotSimplified());
            //StartCoroutine(SpawnRobotRandom());
            //StartCoroutine(SpawnRobotDefault());   
        }

        private IEnumerator SpawnRobotSimplified()
        {
            yield return new WaitForSeconds(initialDelay);

            while (true)
            {
                IRobot robot = robotFactory.CreateRobotSimplified(true);
                robot.gameObject.transform.position = transform.position;

                GameObject packageObject = new GameObject();
                Package package = packageObject.AddComponent<Package>();
                package.Wrap(robot, packageSide);

                RobotSimplified robotSimplified = robot as RobotSimplified;
                float forwardPushForce = Random.Range(forwardPushLimits.x, forwardPushLimits.y);
                float pushAngle = Random.Range(pushAngleLimits.x, pushAngleLimits.y);
                Vector3 pushDirection = Quaternion.Euler(0f, pushAngle, 0f) * transform.forward;
                robotSimplified.Rigidbody.isKinematic = true;
                package.Rigidbody.AddForce(pushDirection * forwardPushForce, ForceMode.VelocityChange);
                
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private IEnumerator SpawnRobotDefault()
        {
            yield return new WaitForSeconds(initialDelay);

            while (true)
            {
                IRobot robot = robotFactory.CreateRobotDefault();
                robot.gameObject.transform.position = transform.position;

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private IEnumerator SpawnRobotRandom()
        {
            yield return new WaitForSeconds(initialDelay);

            while (true)
            {
                IRobot robot = robotFactory.CreateRobotRandom(true);
                robot.gameObject.transform.position = transform.position;

                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}