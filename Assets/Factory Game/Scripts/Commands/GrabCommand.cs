using System;

namespace BaseUnit.Commands
{
    public class GrabCommand<T> : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComlete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        public DisplayableInfo displayableInfo => throw new NotImplementedException();

        private Inventory<T> inventory;
        private readonly T item;

        public GrabCommand(Inventory<T> inventory, T item)
        {
            this.inventory = inventory;
            this.item = item;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke(this);

            inventory.Put(item);

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