using System;
using UnityEngine;
using Environment;
using Resources;

namespace BaseUnit.Commands
{
    public class InteractCommand : Command, IDisplayable
    {
        public override event Action<Command> OnStart;
        public override event Action<Command> OnComplete;
        public override event Action<Command> OnCancel;

        public override CommandState CommandState => commandState;

        private CommandState commandState;

        private readonly IInteractable<Player> interactionTarget;
        private readonly Player interactionSender;

        private Sprite actionSprite;

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

        public Sprite GetActionSprite(CommandVisualisationInfo visualisationInfo)
        {
            if (actionSprite != null) return actionSprite;

            if (interactionTarget is Workbench)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Interact, CommandTarget.Workbench));

            if (interactionTarget is SuppliesPile suppliesPile)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Interact, CommandTarget.SuppliesPile, suppliesPile.SupplieType));

            if (interactionTarget is DropZone)
                actionSprite = visualisationInfo.GetActionIcon(new(CommandType.Interact, CommandTarget.DropZone));

            return actionSprite;
        }
    }
}