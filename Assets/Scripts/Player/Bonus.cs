using UnityEngine;

namespace GoldProject
{
    public interface Bonus
    {
        public enum Type
        {
            ActionPerTurn,
            InteractionRange
        }

        public Bonus.Type type { get; }
        public int value { get; }
    }

    [System.Serializable]
    public class HealthRelatedBonus : Bonus
    {
        [Attributes.ReadOnly] public bool enabled;
        [Range(0f, 1f)] public float healthPercentageNeeded = 0.5f;
        
        // Type
        [SerializeField] private Bonus.Type _type;
        public Bonus.Type type => _type;
        
        // Value
        [SerializeField, Range(1, 5)] private int _value;
        public int value => _value;
    }
}