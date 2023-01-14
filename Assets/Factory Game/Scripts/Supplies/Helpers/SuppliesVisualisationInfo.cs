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

        private Dictionary<SupplieType, Material> sprites = new();

        public Material GetMaterial(SupplieType supplieType)
        {
            sprites.TryGetValue(supplieType, out Material value);
            return value;
        }

        private void OnValidate() => ProcessSerializeData();

        private void ProcessSerializeData()
        {
            sprites.Clear();

            foreach (var item in supplieData)
            {
                if (!sprites.ContainsKey(item.supplieType))
                    sprites.Add(item.supplieType, item.material);
            }
        }
    }

    [Serializable]
    public class SupplieVisualisationData
    {
        public SupplieType supplieType;
        public Material material;

        public SupplieVisualisationData(SupplieType supplieType, Material material)
        {
            this.supplieType = supplieType;
            this.material = material;
        }
    }
}