using System;

namespace BaseUnit.Commands
{
    public class ThrowCommand<T> : Command
    {
        public override event Action OnStart;
        public override event Action OnComlete;
        public override event Action OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        private Inventory<T> inventory;

        public ThrowCommand(Inventory<T> inventory)
        {
            this.inventory = inventory;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke();

            inventory.Remove();

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