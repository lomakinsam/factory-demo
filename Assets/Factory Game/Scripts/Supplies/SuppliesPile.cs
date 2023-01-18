using System.Collections.Generic;
using UnityEngine;
using BaseUnit;
using BaseUnit.Commands;

namespace Resources
{
    public class SuppliesPile : MonoBehaviour, IInteractable<Player>
    {
        [SerializeField] private Supplies suppliesInstance;

        private List<Supplies> suppliesPool;

        public SupplieType SupplieType => suppliesInstance.SuppliesType;

        public void Interact(Player interactionSender)
        {
            Supplies supplies = GetSupplies();
            interactionSender.SetItem(supplies);
        }

        private Supplies GetSupplies()
        {
            if (suppliesPool == null)
                suppliesPool = new();

            foreach (var item in suppliesPool)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                    return item;
                }
            }

            Supplies supplie = Instantiate(suppliesInstance);
            suppliesPool.Add(supplie);
            return supplie;
        }
    }
}