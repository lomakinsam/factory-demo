using System.Collections;
using UnityEngine;

namespace BaseUnit
{
    public class BackgroundCharacter : Unit
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private Transform[] waypoints;

        private readonly int walkAnimBool_ID = Animator.StringToHash("Walk_Anim");
        private readonly int openAnimBool_ID = Animator.StringToHash("Open_Anim");

        private float closeAnimDuration = 1f;
        private float openAnimDuration = 3f;
        private float activationDelay = 5f;
        private float idleToMoveTransitionDuration = 0.5f;

        private int targetWaypointIndex;

        private void Start() => StartCoroutine(Activate(activationDelay));

        private IEnumerator Activate(float delay)
        {
            yield return new WaitForSeconds(delay);

            characterAnimator.SetBool(openAnimBool_ID, true);
            yield return new WaitForSeconds(openAnimDuration);

            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            characterAnimator.SetBool(walkAnimBool_ID, true);
            yield return new WaitForSeconds(idleToMoveTransitionDuration);

            targetWaypointIndex = targetWaypointIndex > 0 ? 0 : waypoints.Length - 1;
            int activeWaypointIndex = (waypoints.Length - 1) - targetWaypointIndex;

            MoveTo(waypoints[activeWaypointIndex].position);

            while (true)
            {
                if (IsCloseToWaypoint(activeWaypointIndex))
                {
                    if (activeWaypointIndex == targetWaypointIndex) break;
                    else if (activeWaypointIndex < targetWaypointIndex) activeWaypointIndex++;
                    else if (activeWaypointIndex > targetWaypointIndex) activeWaypointIndex--;

                    MoveTo(waypoints[activeWaypointIndex].position);
                }

                yield return null;
            }

            characterAnimator.SetBool(walkAnimBool_ID, false);

            StartCoroutine(Deactivate());
        }

        private IEnumerator Deactivate()
        {
            characterAnimator.SetBool(openAnimBool_ID, false);
            yield return new WaitForSeconds(closeAnimDuration);
            transform.Rotate(Vector3.up, 180);

            StartCoroutine(Activate(activationDelay));
        }

        private bool IsCloseToWaypoint(int waypointIndex)
        {
            Vector3 offset = waypoints[waypointIndex].position - transform.position;
            float sqrLen = offset.sqrMagnitude;
            float minDistance = 0.25f;

            if (sqrLen < minDistance * minDistance)
                return true;
            else
                return false;
        }
    }
}