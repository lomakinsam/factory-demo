using System;

namespace BaseUnit.Commands
{
    public class GrabCommand<T> : Command
    {
        public override event Action OnStart;
        public override event Action OnComlete;
        public override event Action OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

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
            OnStart?.Invoke();

            inventory.Put(item);

            commandState = CommandState.Pending;
            OnComlete?.Invoke();
        }

        public override void Cancel()
        {
            commandState = CommandState.Pending;
            OnCancel?.Invoke();
        }
    }
}