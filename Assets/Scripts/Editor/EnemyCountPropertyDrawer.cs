using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomPropertyDrawer(typeof(GameManager.EnemyCount))]
    public class EnemyCountPropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect half = new Rect(position);
            half.width *= 0.5f;
            EditorGUI.PropertyField(half, property.FindPropertyRelative("enemyType"), GUIContent.none);
            
            half.x += half.width + 10;
            half.width -= 20;
            half.height -= 5;
            EditorGUI.PropertyField(half, property.FindPropertyRelative("count"), GUIContent.none);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}