using UnityEngine;

namespace BaseUnit.Commands
{
    public interface IDisplayable
    {
        public DisplayableInfo displayableInfo { get; }
    }

    [CreateAssetMenu(menuName = "Displayable/Displayable Info")]
    public class DisplayableInfo : ScriptableObject
    {
        public string description;
        public Sprite sprite;
    }
}