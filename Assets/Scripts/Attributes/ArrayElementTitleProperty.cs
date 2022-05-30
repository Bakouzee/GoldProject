using UnityEngine;
namespace GoldProject.Attributes
{
    public class ArrayElementTitleProperty : PropertyAttribute
    {
        public string nameBase;
        public ArrayElementTitleProperty(string nameBase) => this.nameBase = nameBase;
    }
}