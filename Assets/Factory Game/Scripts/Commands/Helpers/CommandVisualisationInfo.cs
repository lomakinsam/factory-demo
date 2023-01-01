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

        private Dictionary<CommandVisualisationDataRequest, Sprite> sprites = new();

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
            foreach (KeyValuePair<CommandVisualisationDataRequest, Sprite> entry in sprites)
            {
                CommandVisualisationData item;

                if (entry.Key is CommandVisualisationDataRequest_SuppliesAsTarget customData)
                    item = new CommandVisualisationData_SuppliesAsTarget(customData.commandTarget, entry.Value, customData.suppliesType);
                else
                    item = new CommandVisualisationData(entry.Key.commandTarget, entry.Value);

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
                    commandsData[i] = new CommandVisualisationData();

                commandsData[i] = CastToCustomData(commandsData[i]);

                CommandVisualisationDataRequest request;

                if (commandsData[i] is CommandVisualisationData_SuppliesAsTarget customData)
                    request = new CommandVisualisationDataRequest_SuppliesAsTarget(commandType, customData.commandTarget, customData.suppliesType);
                else
                    request = new CommandVisualisationDataRequest(commandType, commandsData[i].commandTarget);

                sprites.Add(request, commandsData[i].actionIcon);
            }
        }

        private CommandVisualisationData CastToCustomData(CommandVisualisationData data)
        {
            if ((data.commandTarget == CommandTarget.SuppliesPile || data.commandTarget == CommandTarget.Supplies) && data is not CommandVisualisationData_SuppliesAsTarget)
                data = new CommandVisualisationData_SuppliesAsTarget(data.commandTarget, data.actionIcon, SuppliesType.Pink);
            else if ((data.commandTarget != CommandTarget.SuppliesPile || data.commandTarget != CommandTarget.Supplies) && data is CommandVisualisationData_SuppliesAsTarget)
                data = new CommandVisualisationData(data.commandTarget, data.actionIcon);

            return data;
        }

        public Sprite GetActionIcon(CommandVisualisationDataRequest request)
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
        public SuppliesType suppliesType;

        public CommandVisualisationData_SuppliesAsTarget(CommandTarget commandTarget, Sprite actionIcon, SuppliesType suppliesType) : base(commandTarget, actionIcon)
        {
            this.suppliesType = suppliesType;
        }
    }

    public class CommandVisualisationDataRequest
    {
        public CommandType commandType;
        public CommandTarget commandTarget;

        public CommandVisualisationDataRequest(CommandType commandType, CommandTarget commandTarget)
        {
            this.commandType = commandType;
            this.commandTarget = commandTarget;
        }
    }

    public class CommandVisualisationDataRequest_SuppliesAsTarget : CommandVisualisationDataRequest
    {
        public SuppliesType suppliesType;

        public CommandVisualisationDataRequest_SuppliesAsTarget(CommandType commandType, CommandTarget commandTarget, SuppliesType suppliesType) : base(commandType, commandTarget)
        {
            this.suppliesType = suppliesType;
        }
    }
}