using UnityEngine;

namespace OutFoxeed.Attributes
{
    public class LabelAttribute : PropertyAttribute
    {
        public string label;
        public LabelAttribute(string label) => this.label = label;
    }
}