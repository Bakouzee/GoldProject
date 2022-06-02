using UnityEditor;
using UnityEngine;

namespace OutFoxeed.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(OutFoxeed.Attributes.LabelAttribute))]
    public class LabelAttributePropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            OutFoxeed.Attributes.LabelAttribute labelAttribute = attribute as OutFoxeed.Attributes.LabelAttribute;
            if (attribute == null)
                return;
            
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.label));
        }
    }
}