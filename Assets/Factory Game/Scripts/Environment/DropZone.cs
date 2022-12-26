using UnityEngine;
using BaseUnit;
using BaseUnit.Commands;

namespace Environment
{
    public class DropZone : MonoBehaviour, IInteractable<Player>
    {
        public void Interact(Player interactionSender)
        {
            throw new System.NotImplementedException();
        }
    }
}