using System;

namespace BaseUnit.Commands
{
    public class ThrowCommand<T> : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComlete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        public DisplayableInfo displayableInfo => throw new NotImplementedException();

        private Inventory<T> inventory;

        public ThrowCommand(Inventory<T> inventory)
        {
            this.inventory = inventory;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke(this);

            inventory.Remove();

            commandState = CommandState.Pending;
            OnComlete?.Invoke(this);
        }

        public override void Cancel()
        {
            commandState = CommandState.Pending;
            OnCancel?.Invoke(this);
        }
    }
}