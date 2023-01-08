using UnityEngine;
using System.Collections.Generic;
using BaseUnit.Commands;
using ModularRobot;
using Resources;
using Environment;

namespace BaseUnit
{
    public class Player : Unit
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private Transform inventorySlot;

        private List<Command> commandsList;
        private const int maxDisplayableCommands = 5;

        private Inventory<Component> playerInventory;

        public bool IsCarryingRepairedRobot
        {
            get
            {
                if (playerInventory.StoredItem == null) return false;

                if (playerInventory.StoredItem is RobotSimplified robotSimplified)
                {
                    if (robotSimplified.DamageStatus == null) return true;
                    else return false;
                }
                else return false;
            }
        }

        private int displayableCommandsCount
        {
            get
            {
                int result = 0;

                foreach (var command in commandsList)
                {
                    if (command is IDisplayable) result++;
                }

                return result;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            commandsList = new();
            playerInventory = new();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                ReceiveCommands();
        }

        public void SetItem(Component item)
        {
            if (!playerInventory.IsEmpty)
            {
                Component currentItem = playerInventory.StoredItem;
                playerInventory.Clear();

                currentItem.transform.parent = null;
                if (currentItem is IPhysical _physicalItem) _physicalItem.EnablePhysics();
            }

            playerInventory.Put(item);

            if (item is IPhysical physicalItem) physicalItem.DisablePhysics();

            item.transform.SetParent(inventorySlot, true);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        public Component GetItem()
        {
            if (playerInventory.IsEmpty) return null;

            Component item = playerInventory.StoredItem;
            playerInventory.Clear();

            item.transform.parent = null;

            return item;
        }

        private void ReceiveCommands()
        {
            if (displayableCommandsCount >= maxDisplayableCommands) return;

            var ray = gameCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Package package = hit.transform.GetComponent<Package>();
                if (package != null)
                {
                    GrabbableObjectRaycastHitResponse(package);
                    return;
                }

                SuppliesPile suppliesPile = hit.transform.GetComponent<SuppliesPile>();
                if (suppliesPile != null)
                {
                    SuppliesPileRaycastHitResponse(suppliesPile);
                    return;
                }

                Workbench workbench = hit.transform.GetComponent<Workbench>();
                if (workbench != null)
                {
                    WorkbenchRaycastHitResponse(workbench);
                    return;
                }

                DropZone dropZone = hit.transform.GetComponent<DropZone>();
                if (dropZone != null)
                {
                    DropZoneRaycastHitResponse(dropZone);
                    return;
                }

                RobotSimplified robot = hit.transform.GetComponent<RobotSimplified>();
                if (robot != null)
                {
                    GrabbableObjectRaycastHitResponse(robot);
                    return;
                }

                Supplies supplies = hit.transform.GetComponent<Supplies>();
                if (supplies != null)
                {
                    GrabbableObjectRaycastHitResponse(supplies);
                    return;
                }
            }
        }

        private void GrabbableObjectRaycastHitResponse(Component grabbableObject)
        {
            // Preventing repetitive actions with the same object
            foreach (var command in commandsList)
            {
                if (command is GrabCommand grabAction && grabAction.Target == grabbableObject)
                    return;
            }

            Command moveCommand = new MoveCommand(navMeshAgent, grabbableObject.transform.position);
            commandsList.Add(moveCommand);

            if (commandsList.Count == 1) moveCommand.Execute();

            Command grabCommand = new GrabCommand(this, grabbableObject);
            commandsList.Add(grabCommand);

            moveCommand.OnComplete += SwitchToNextCommand;
            moveCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: grabCommand, preliminaryCommands: new Command[] { moveCommand }); };

            grabCommand.OnComplete += SwitchToNextCommand;
            grabCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: grabCommand, preliminaryCommands: new Command[] { moveCommand }); };

            UpdateUICommandsPanel();
        }

        private void SuppliesPileRaycastHitResponse(SuppliesPile suppliesPile)
        {
            Command moveCommand = new MoveCommand(navMeshAgent, suppliesPile.transform.position);
            commandsList.Add(moveCommand);

            if (commandsList.Count == 1) moveCommand.Execute();

            Command interactCommand = new InteractCommand(suppliesPile, this);
            commandsList.Add(interactCommand);

            moveCommand.OnComplete += SwitchToNextCommand;
            moveCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            interactCommand.OnComplete += SwitchToNextCommand;
            interactCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            UpdateUICommandsPanel();
        }

        private void WorkbenchRaycastHitResponse(Workbench workbench)
        {
            Command moveCommand = new MoveCommand(navMeshAgent, workbench.transform.position);
            commandsList.Add(moveCommand);

            if (commandsList.Count == 1) moveCommand.Execute();

            Command interactCommand = new InteractCommand(workbench, this);
            commandsList.Add(interactCommand);

            moveCommand.OnComplete += SwitchToNextCommand;
            moveCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            interactCommand.OnComplete += SwitchToNextCommand;
            interactCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            UpdateUICommandsPanel();
        }

        private void DropZoneRaycastHitResponse(DropZone dropZone)
        {
            Command moveCommand = new MoveCommand(navMeshAgent, dropZone.transform.position);
            commandsList.Add(moveCommand);

            if (commandsList.Count == 1) moveCommand.Execute();

            Command interactCommand = new InteractCommand(dropZone, this);
            commandsList.Add(interactCommand);

            moveCommand.OnComplete += SwitchToNextCommand;
            moveCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            interactCommand.OnComplete += SwitchToNextCommand;
            interactCommand.OnCancel += delegate { CancelCommandsChain(mainCommand: interactCommand, preliminaryCommands: new Command[] { moveCommand }); };

            UpdateUICommandsPanel();
        }


        private void SwitchToNextCommand(Command executedCommand)
        {
            commandsList.Remove(executedCommand);

            if (commandsList.Count > 0)
                commandsList[0].Execute();

            UpdateUICommandsPanel();
        }

        private void CancelCommandsChain(Command mainCommand, Command[] preliminaryCommands)
        {
            if (!commandsList.Contains(mainCommand)) return;

            commandsList.Remove(mainCommand);

            for (int i = 0; i < preliminaryCommands.Length; i++)
            {
                if (!commandsList.Contains(preliminaryCommands[i])) continue;

                if (preliminaryCommands[i].CommandState == CommandState.Executing)
                    preliminaryCommands[i].Cancel();

                commandsList.Remove(preliminaryCommands[i]);
            }

            UpdateUICommandsPanel();
        }

        private void UpdateUICommandsPanel() => DebugCommandList();

        private void DebugCommandList()
        {
            string debugMassage = "Commands: ";

            foreach (var command in commandsList)
            {
                debugMassage += $"{command}";

                if (command is IDisplayable)
                    debugMassage += " (I) -> ";
                else
                    debugMassage += " -> ";
            }

            Debug.Log(debugMassage);
        }
    }
}