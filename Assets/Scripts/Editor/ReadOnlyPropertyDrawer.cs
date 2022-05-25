using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomPropertyDrawer(typeof(Attributes.ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (attribute is Attributes.ReadOnlyAttribute readOnlyAttribute)
            {
                if (!readOnlyAttribute.readOnly)
                    return;

                var previousGUI = GUI.enabled;

                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = previousGUI;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}