using UnityEngine;

namespace GoldProject.Attributes
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public bool readOnly = true;

        public ReadOnlyAttribute() => this.readOnly = true;
        public ReadOnlyAttribute(bool readOnly) => this.readOnly = readOnly;
    }
}