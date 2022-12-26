using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using BaseUnit;
using BaseUnit.Commands;

namespace Environment
{
    public class Workbench : MonoBehaviour, IInteractable<Player>
    {
        [Header("General Setup")]
        [SerializeField] private Transform[] supplieSlots;
        [SerializeField] private Transform brokenRobotSlot;

        [Header("Robotic Hand Setup")]
        [SerializeField] private Animator roboticHandAnimator;
        [SerializeField] private VisualEffect sparksEffect;

        private readonly int weldingAnimTrigger_ID = Animator.StringToHash("Welding");

        private const float weldingAnimLength = 6.54f;
        private const float weldingStartStep = 0.2f;
        private const float weldingEndStep = 0.75f;
        private const float weldingStartTime = weldingAnimLength * weldingStartStep;
        private const float weldingEndTime = weldingAnimLength * weldingEndStep - weldingStartTime;
        private const float enclosingTime = weldingAnimLength - weldingStartTime - weldingEndTime;

        private void Awake() => Init();

        private void OnMouseDown()
        {
            StartCoroutine(RoboticHandAnimate(4));
        }

        private void Init()
        {
            sparksEffect.Stop();
        }

        private IEnumerator RoboticHandAnimate(int cycles)
        {
            for (int i = 0; i < cycles; i++)
            {
                roboticHandAnimator.SetTrigger(weldingAnimTrigger_ID);

                yield return new WaitForSeconds(weldingStartTime);
                sparksEffect.Play();

                yield return new WaitForSeconds(weldingEndTime);
                sparksEffect.Stop();

                yield return new WaitForSeconds(enclosingTime);
            }
        }

        public void Interact(Player interactionSender)
        {
            throw new System.NotImplementedException();
        }
    }
}