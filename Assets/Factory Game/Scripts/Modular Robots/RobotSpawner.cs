using UnityEngine;
using System.Collections;

namespace ModularRobot
{
    public class RobotSpawner : MonoBehaviour
    {
        [SerializeField]
        private RobotFactory robotFactory;
        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float spawnDelay;

        private Coroutine spawnCycle;

        private void Start()
        {
            spawnCycle = StartCoroutine(SpawnRobotSimplified());
            //StartCoroutine(SpawnRobotRandom());
            //StartCoroutine(SpawnRobotDefault());   
        }

        private IEnumerator SpawnRobotSimplified()
        {
            while (true)
            {
                IRobot robot = robotFactory.CreateRobotSimplified();
                robot.gameObject.transform.position = transform.position;

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private IEnumerator SpawnRobotDefault()
        {
            while (true)
            {
                IRobot robot = robotFactory.CreateRobotDefault();
                robot.gameObject.transform.position = transform.position;

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        private IEnumerator SpawnRobotRandom()
        {
            while (true)
            {
                IRobot robot = robotFactory.CreateRobotRandom(true);
                robot.gameObject.transform.position = transform.position;

                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}