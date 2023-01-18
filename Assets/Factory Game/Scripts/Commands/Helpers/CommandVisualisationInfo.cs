using System;
using System.Collections.Generic;
using UnityEngine;
using Resources;

namespace BaseUnit.Commands
{
    [CreateAssetMenu(menuName = "Displayable/Commands Visualisation Info")]
    public class CommandVisualisationInfo : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeReference]
        private List<CommandVisualisationData> grab;
        [SerializeReference]
        private List<CommandVisualisationData> interact;

        private Dictionary<Request, Sprite> sprites = new();

        public void OnBeforeSerialize()
        {
            grab.Clear();
            interact.Clear();

            ToSerializedData();
        }

        public void OnAfterDeserialize()
        {
            sprites.Clear();

            if (grab.Count > 0)
                ReadFromSerializedData(ref grab, CommandType.Grab);
            if (interact.Count > 0)
                ReadFromSerializedData(ref interact, CommandType.Interact);
        }

        private void ToSerializedData()
        {
            foreach (KeyValuePair<Request, Sprite> entry in sprites)
            {
                CommandVisualisationData item;

                if (entry.Key.supplieType != null)
                    item = new CommandVisualisationData_SuppliesAsTarget((CommandTarget)entry.Key.commandTarget, entry.Value, (SupplieType)entry.Key.supplieType);
                else
                   item = new CommandVisualisationData((CommandTarget)entry.Key.commandTarget, entry.Value);

                if (entry.Key.commandType == CommandType.Grab)
                    grab.Add(item);

                if (entry.Key.commandType == CommandType.Interact)
                    interact.Add(item);
            }
        }

        private void ReadFromSerializedData(ref List<CommandVisualisationData> commandsData, CommandType commandType)
        {
            for (int i = 0; i < commandsData.Count; i++)
            {
                if (commandsData[i] == null)
                {
                    var emptyItem = GenerateUniqueItem(commandType);

                    if (emptyItem == null)
                        continue;
                    else
                        commandsData[i] = emptyItem;
                }
                    
                commandsData[i] = CastToCustomData(commandsData[i]);

                Request request;

                if (commandsData[i] is CommandVisualisationData_SuppliesAsTarget customData)
                    request = new Request(commandType, customData.commandTarget, customData.suppliesType);
                else
                    request = new Request(commandType, commandsData[i].commandTarget);

                if (!sprites.ContainsKey(request))
                    sprites.Add(request, commandsData[i].actionIcon);
                else
                {
                    var subitem = GenerateUniqueSubitem((CommandType)request.commandType, (CommandTarget)request.commandTarget);

                    if (subitem == null) return;

                    request.supplieType = subitem.suppliesType;
                    sprites.Add(request, commandsData[i].actionIcon);
                }
            }
        }

        private CommandVisualisationData GenerateUniqueItem(CommandType commandType)
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(CommandTarget)).Length; i++)
            {
                CommandTarget commandTarget = (CommandTarget)i;

                if (commandTarget != CommandTarget.SuppliesPile && commandTarget != CommandTarget.Supplies)
                {
                    var request = new Request(commandType, commandTarget);

                    if (sprites.ContainsKey(request))
                        continue;
                    else
                        return new CommandVisualisationData(commandTarget, null);
                }
                else
                    return GenerateUniqueSubitem(commandType, commandTarget);
            }

            return null;
        }

        private CommandVisualisationData_SuppliesAsTarget GenerateUniqueSubitem(CommandType commandType, CommandTarget commandTarget)
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(SupplieType)).Length; i++)
            {
                SupplieType supplieType = (SupplieType)i;

                var request = new Request(commandType, commandTarget, supplieType);

                if (sprites.ContainsKey(request))
                    continue;
                else
                    return new CommandVisualisationData_SuppliesAsTarget(commandTarget, null, supplieType);
            }

            return null;
        }

        private CommandVisualisationData CastToCustomData(CommandVisualisationData data)
        {
            if ((data.commandTarget == CommandTarget.SuppliesPile || data.commandTarget == CommandTarget.Supplies) && data is not CommandVisualisationData_SuppliesAsTarget)
                data = new CommandVisualisationData_SuppliesAsTarget(data.commandTarget, data.actionIcon, SupplieType.Pink);
            else if (!(data.commandTarget == CommandTarget.SuppliesPile || data.commandTarget == CommandTarget.Supplies) && data is CommandVisualisationData_SuppliesAsTarget)
                data = new CommandVisualisationData(data.commandTarget, data.actionIcon);

            return data;
        }

        public Sprite GetActionIcon(Request request)
        {
            Sprite value;
            sprites.TryGetValue(request, out value);
            return value;
        }
    }

    public enum CommandType { Grab, Interact }
    public enum CommandTarget { Package, Workbench, SuppliesPile, Supplies, RepairableRobot, DropZone }

    [Serializable]
    public class CommandVisualisationData
    {
        public CommandTarget commandTarget;
        public Sprite actionIcon;

        public CommandVisualisationData()
        {
            this.commandTarget = CommandTarget.Package;
            this.actionIcon = null;
        }

        public CommandVisualisationData(CommandTarget commandTarget, Sprite actionIcon)
        {
            this.commandTarget = commandTarget;
            this.actionIcon = actionIcon;
        }
    }

    [Serializable]
    public class CommandVisualisationData_SuppliesAsTarget : CommandVisualisationData
    {
        public SupplieType suppliesType;

        public CommandVisualisationData_SuppliesAsTarget(CommandTarget commandTarget, Sprite actionIcon, SupplieType suppliesType) : base(commandTarget, actionIcon)
        {
            this.suppliesType = suppliesType;
        }
    }

    public class Request
    {
        public CommandType? commandType;
        public CommandTarget? commandTarget;
        public SupplieType? supplieType;

        public Request(CommandType commandType, CommandTarget commandTarget)
        {
            this.commandType = commandType;
            this.commandTarget = commandTarget;
            this.supplieType = null;
        }

        public Request(CommandType commandType, CommandTarget commandTarget, SupplieType supplieType)
        {
            this.commandType = commandType;
            this.commandTarget = commandTarget;
            this.supplieType = supplieType;
        }

        public override int GetHashCode()
        {
            int commandTypeInt = commandType == null ? 0 : (int)commandType;
            int commandTargetInt = commandTarget == null ? 0 : (int)commandTarget;
            int supplieTypeInt = supplieType == null ? 0 : (int)supplieType;

            return commandTypeInt ^ commandTargetInt ^ supplieTypeInt;
        }

        public override bool Equals(object obj) => Equals(obj as Request);

        public bool Equals(Request obj) => obj != null && obj.commandType == commandType && obj.commandTarget == commandTarget && obj.supplieType == supplieType;
    }
}