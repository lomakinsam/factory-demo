using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Displayable/Supplies Visualisation Info")]
    public class SuppliesVisualisationInfo : ScriptableObject
    {
        [SerializeField]
        private List<SupplieVisualisationData> supplieData;

        private Dictionary<SupplieType, Sprite> sprites = new();

        public Sprite SupplieIcon(SupplieType supplieType)
        {
            sprites.TryGetValue(supplieType, out Sprite value);
            return value;
        }

        private void OnValidate() => ProcessSerializeData();

        private void ProcessSerializeData()
        {
            sprites.Clear();

            foreach (var item in supplieData)
            {
                if (!sprites.ContainsKey(item.supplieType))
                    sprites.Add(item.supplieType, item.icon);
            }
        }
    }

    [Serializable]
    public class SupplieVisualisationData
    {
        public SupplieType supplieType;
        public Sprite icon;

        public SupplieVisualisationData(SupplieType supplieType, Sprite icon)
        {
            this.supplieType = supplieType;
            this.icon = icon;
        }
    }
}