using System;
using BaseUnit;

namespace BaseUnit.Commands
{
    public class InteractCommand<T> : Command
    {
        public override event Action OnStart;
        public override event Action OnComlete;
        public override event Action OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        private readonly IInteractable<Player> interactionTarget;
        private readonly Player interactionSender;

        public InteractCommand(IInteractable<Player> interactionTarget, Player interactionSender)
        {
            this.interactionTarget = interactionTarget;
            this.interactionSender = interactionSender;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke();

            interactionTarget.Interact(interactionSender);

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