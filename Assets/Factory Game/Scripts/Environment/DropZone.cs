using UnityEngine;
using System.Collections;
using BaseUnit;
using BaseUnit.Commands;
using ModularRobot;

namespace Environment
{
    public class DropZone : MonoBehaviour, IInteractable<Player>
    {
        private const float destructionDelay = 0.5f;

        public void Interact(Player interactionSender)
        {
            if (interactionSender.IsCarryingRepairedRobot)
            {
                RobotSimplified robot = interactionSender.GetItem() as RobotSimplified;
                robot.EnablePhysics();
                StartCoroutine(DestroyDelayed(robot.gameObject));
            }
        }

        private IEnumerator DestroyDelayed(GameObject gameObject)
        {
            yield return new WaitForSeconds(destructionDelay);
            Destroy(gameObject);
        }
    }
}