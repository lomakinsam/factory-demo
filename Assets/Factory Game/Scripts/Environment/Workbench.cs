using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using BaseUnit;
using BaseUnit.Commands;
using ModularRobot;
using Resources;

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

        private const float packageUnpackDelay = 0.5f;

        private Coroutine roboticHandAnimation;

        private RobotSimplified brokenRobotItem;
        private Supplies[] supplieItems;
        private List<SupplieType> requiredSupplies;

        private SupplieType RandomSuppliesType => (SupplieType)Random.Range(0, System.Enum.GetValues(typeof(SupplieType)).Length);

        private void Awake() => Init();

        private void Init()
        {
            sparksEffect.Stop();

            brokenRobotItem = null;
            requiredSupplies = new();

            supplieItems = new Supplies[supplieSlots.Length];
            for (int i = 0; i < supplieItems.Length; i++)
                supplieItems[i] = null;
        }

        private IEnumerator RoboticHandAnimate()
        {
            roboticHandAnimator.SetTrigger(weldingAnimTrigger_ID);

            yield return new WaitForSeconds(weldingStartTime);
            sparksEffect.Play();

            yield return new WaitForSeconds(weldingEndTime);
            sparksEffect.Stop();

            yield return new WaitForSeconds(enclosingTime);

            roboticHandAnimation = null;
        }

        public void Interact(Player interactionSender)
        {
            if (brokenRobotItem != null && brokenRobotItem.DamageStatus == null)
            {
                brokenRobotItem.gameObject.transform.parent = null;
                interactionSender.SetItem(brokenRobotItem);
                return;
            }

            Component receivedItem = interactionSender.GetItem();

            if (receivedItem == null) return;

            if (receivedItem is Package package)
            {
                ReceivePackage(package, interactionSender);
                return;
            }

            if (receivedItem is Supplies supplies)
            {
                ReceiveSupplies(supplies, interactionSender);
                return;
            }
        }

        private void ReceiveSupplies(Supplies supplies, Player interactionSender)
        {
            if (!(requiredSupplies.Count > 0 && requiredSupplies.Contains(supplies.SuppliesType)))
            {
                interactionSender.SetItem(supplies);
                return;
            }

            for (int i = 0; i < supplieSlots.Length; i++)
            {
                if (supplieSlots[i].childCount == 0 && supplieItems[i] == null)
                {
                    supplies.transform.SetParent(supplieSlots[i], true);
                    supplies.transform.localPosition = Vector3.zero;
                    supplies.transform.localRotation = Quaternion.identity;

                    supplieItems[i] = supplies;
                    requiredSupplies.Remove(supplies.SuppliesType);

                    break;
                }
            }

            if (requiredSupplies.Count == 0)
                RepairBrokenRobot();
        }

        private void ReceivePackage(Package package, Player interactionSender)
        {
            if (brokenRobotItem != null || brokenRobotSlot.childCount > 0)
            {
                interactionSender.SetItem(package);
                return;
            }

            StartCoroutine(_ReceivePackage(package));
        }

        private IEnumerator _ReceivePackage(Package package)
        {
            package.transform.SetParent(brokenRobotSlot, true);
            package.transform.localPosition = Vector3.zero;
            package.transform.localRotation = Quaternion.identity;

            yield return new WaitForSeconds(packageUnpackDelay);

            brokenRobotItem = package.Unwrap() as RobotSimplified;
            brokenRobotItem.gameObject.transform.SetParent(brokenRobotSlot, true);

            GenerateRequiredSupplies();
        }

        private void GenerateRequiredSupplies()
        {
            if (requiredSupplies.Count > 0) return;

            if (brokenRobotItem.GetDamageStatus(ModuleType.Hull) != null)
                requiredSupplies.Add(RandomSuppliesType);
            if (brokenRobotItem.GetDamageStatus(ModuleType.Chassis) != null)
                requiredSupplies.Add(RandomSuppliesType);
        }

        private void RepairBrokenRobot()
        {
            if (brokenRobotItem != null)
                StartCoroutine(_RepairBrokenRobot());
        }

        private IEnumerator _RepairBrokenRobot()
        {
            while (brokenRobotItem != null && brokenRobotItem.DamageStatus != null)
            {
                roboticHandAnimation = StartCoroutine(RoboticHandAnimate());

                yield return new WaitForSeconds(weldingAnimLength * weldingEndStep);

                DamageType? repairedModuleDamageStatus = brokenRobotItem.Repair();
                if (repairedModuleDamageStatus == null) RemoveSupplie();

                yield return new WaitWhile(() => roboticHandAnimation != null);
            }
        }

        private void RemoveSupplie()
        {
            for (int i = 0; i < supplieItems.Length; i++)
            {
                if (supplieItems[i] != null)
                {
                    supplieItems[i].transform.parent = null;
                    supplieItems[i].gameObject.SetActive(false);
                    supplieItems[i] = null;

                    return;
                }
            }
        }
    }
}