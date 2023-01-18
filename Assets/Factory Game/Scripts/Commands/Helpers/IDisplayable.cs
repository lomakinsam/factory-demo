using UnityEngine;

namespace BaseUnit.Commands
{
    public interface IDisplayable
    {
        public Sprite GetActionSprite(CommandVisualisationInfo visualisationInfo);
    }
}