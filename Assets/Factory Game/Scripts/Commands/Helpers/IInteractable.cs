namespace BaseUnit.Commands
{
    public interface IInteractable<T> where T : UnityEngine.Object
    {
        public void Interact(T interactionSender);
    }
}