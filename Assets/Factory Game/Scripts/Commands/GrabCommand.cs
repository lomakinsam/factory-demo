using System;
using UnityEngine;

namespace BaseUnit.Commands
{
    public class GrabCommand : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComplete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        public DisplayableInfo displayableInfo => throw new NotImplementedException();

        private readonly Player player;
        private readonly Component item;

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
    }
}