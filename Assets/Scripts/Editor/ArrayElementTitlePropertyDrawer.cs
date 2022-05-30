using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomPropertyDrawer(typeof(Attributes.ArrayElementTitleProperty))]
    public class ArrayElementTitlePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            Attributes.ArrayElementTitleProperty arrayElementTitleProperty =
                attribute as Attributes.ArrayElementTitleProperty;
            if (arrayElementTitleProperty == null)
                return;
            EditorGUI.PropertyField(position, property, new GUIContent(arrayElementTitleProperty.nameBase));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}