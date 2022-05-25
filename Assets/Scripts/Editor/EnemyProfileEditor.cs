using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomEditor(typeof(EnemyProfile))]
    public class EnemyProfileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);
            
            EnemyProfile enemyProfile = target as EnemyProfile;

            Texture2D texture = AssetPreview.GetAssetPreview(enemyProfile.sprite);
            if (texture == null)
                return;
            GUILayout.Label("", GUILayout.Width(200), GUILayout.Height(200));
            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

        }
    }
}