using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label(GameManager.dayState.ToString());
            base.OnInspectorGUI();
        }
    }
}