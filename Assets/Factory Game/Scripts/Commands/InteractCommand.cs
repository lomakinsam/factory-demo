using System;

namespace BaseUnit.Commands
{
    public class InteractCommand<T> : Command
    {
        public override event Action OnStart;
        public override event Action OnComlete;
        public override event Action OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        private readonly IInteractable<T> interactionTarget;
        private readonly T interactionInfo;

        public InteractCommand(IInteractable<T> interactionTarget, T interactionInfo)
        {
            this.interactionTarget = interactionTarget;
            this.interactionInfo = interactionInfo;
        }

        public override void Execute()
        {
            commandState = CommandState.Executing;
            OnStart?.Invoke();

            interactionTarget.Interact(interactionInfo);

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