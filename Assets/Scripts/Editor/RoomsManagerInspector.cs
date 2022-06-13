using GoldProject.Rooms;
using GridSystem;
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

            if (Application.isPlaying)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Check all pathpoints"))
                {
                    GridManager gridManager = GridManager.Instance;
                    bool allValid = true;
                    foreach (var room in roomsManager.Rooms)
                    {
                        foreach (var pathPoint in room.pathPoints)
                        {
                            if (!gridManager.GetTileAtPosition(pathPoint.position))
                            {
                                Debug.LogWarning($"{pathPoint.name} is invalid", pathPoint);
                                allValid = false;
                            }
                        }
                    }
                    if(allValid) Debug.Log("<color=green>Each pathpoint registered in the rooms manager is valid</color>");
                }
            }
        }
    }
}