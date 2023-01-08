using System;

namespace BaseUnit.Commands
{
    public class InteractCommand : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComplete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;
        private CommandState commandState;

        public DisplayableInfo displayableInfo => throw new NotImplementedException();

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
            OnStart?.Invoke(this);

            interactionTarget.Interact(interactionSender);

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