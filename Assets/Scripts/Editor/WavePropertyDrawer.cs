using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomPropertyDrawer(typeof(GameManager.Wave))]
    public class WavePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty spWaves = property.serializedObject.FindProperty("waves");
            int index = 0;
            for (int i = 0; i < spWaves.arraySize; i++)
            {
                SerializedProperty sp = spWaves.GetArrayElementAtIndex(i);
                if (sp.propertyPath == property.propertyPath)
                {
                    index = i;
                    break;
                }
            }

            EditorGUI.PropertyField(position, property.FindPropertyRelative("enemyCounts"),
                new GUIContent($"Wave {index + 1}"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty spEnemyCounts = property.FindPropertyRelative("enemyCounts");
            float height = base.GetPropertyHeight(property, label);
            if (spEnemyCounts.isExpanded)
                height += (spEnemyCounts.arraySize + 2) * 24;
            return height;
        }
    }
}