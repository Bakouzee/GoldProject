// using UnityEditor;
// using UnityEngine;
//
// namespace GoldProject.Editor
// {
//     [CustomPropertyDrawer(typeof(GameManager.Wave))]
//     public class WavePropertyDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property,
//             GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);
//
//             SerializedProperty spWaves = property.serializedObject.FindProperty("waves");
//             int index = 0;
//             for (int i = 0; i < spWaves.arraySize; i++)
//             {
//                 SerializedProperty sp = spWaves.GetArrayElementAtIndex(i);
//                 if (sp.propertyPath == property.propertyPath)
//                 {
//                     index = i;
//                     break;
//                 }
//             }
//             GUIContent headerGuiContent = new GUIContent($"Wave {index + 1}");
//
//             // Foldout
//             Rect foldoutRect = new Rect(position.x, position.y, position.width, 18);
//             property.isExpanded = true;
//                 // EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, headerGuiContent);
//             if (property.isExpanded)
//             {
//                 
//                 EditorGUI.PropertyField(position, property.FindPropertyRelative("chief"));
//                 EditorGUI.PropertyField(position, property.FindPropertyRelative("enemies"));
//             }
//             // EditorGUI.EndFoldoutHeaderGroup();
//             
//             EditorGUI.EndProperty();
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             SerializedProperty spChief = property.FindPropertyRelative("chief");
//             SerializedProperty spEnemyCounts = property.FindPropertyRelative("enemies");
//             float height = base.GetPropertyHeight(property, label);
//             if (spEnemyCounts.isExpanded)
//                 height += (spEnemyCounts.arraySize + 2) * 24;
//             if (spChief.isExpanded)
//                 height += (spChief.arraySize + 2) * 24;
//             return height;
//         }
//     }
// }