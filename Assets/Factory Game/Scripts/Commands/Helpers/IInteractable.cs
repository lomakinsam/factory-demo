namespace BaseUnit.Commands
{
    public interface IInteractable<T>
    {
        public void Interact(T interactionInfo);
    }
}