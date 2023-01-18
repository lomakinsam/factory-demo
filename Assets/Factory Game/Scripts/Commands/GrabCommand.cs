using System;
using UnityEngine;
using ModularRobot;
using Resources;

namespace BaseUnit.Commands
{
    public class GrabCommand : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComplete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        public Component Target => item;

        private readonly Player player;
        private readonly Component item;

        private Sprite actionSprite;

        public GrabCommand(Player player, Component item)
        {
            this.player = player;
            this.item = item;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke(this);

            player.SetItem(item);

            commandState = CommandState.Pending;
            OnComplete?.Invoke(this);
        }

        public override void Cancel()
        {
            commandState = CommandState.Pending;
            OnCancel?.Invoke(this);
        }

        public Sprite GetActionSprite(CommandVisualisationInfo visualisationInfo)
        {
            if (actionSprite != null) return actionSprite;

            if (item is Package)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Grab, CommandTarget.Package));

            if (item is RobotSimplified)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Grab, CommandTarget.RepairableRobot));

            Supplies itemToSupplies = item as Supplies;
            if (itemToSupplies != null)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Grab, CommandTarget.Supplies, itemToSupplies.SuppliesType));

            return actionSprite;
        }
    }
}