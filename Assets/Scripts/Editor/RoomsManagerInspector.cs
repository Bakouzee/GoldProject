using GoldProject.Rooms;
using UnityEditor;
using UnityEngine;

namespace GoldProject.Editor
{
    [CustomEditor(typeof(RoomsManager))]
    public class RoomsManagerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(10);
            
            RoomsManager roomsManager = target as RoomsManager;
            if (roomsManager == null)
                return;
            if (GUILayout.Button("Helper"))
            {
                foreach (Room room in roomsManager.Rooms)
                {
                    if (room == null)
                        continue;
                    if (room.roomTransform == null)
                        continue;

                    if (room.roomTransform.TryGetComponent(out BoxCollider2D boxCollider2D))
                    {
                        room.size = new Vector2Int(Mathf.RoundToInt(boxCollider2D.size.x),
                            Mathf.RoundToInt(boxCollider2D.size.y));
                    }
                }
            }
        }
    }
}