using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace GridSystem
{
    public abstract class GridController : MonoBehaviour
    {
        private GridManager gridManager;
        private Vector2Int gridPosition;
        protected virtual void Start()
        {
            gridManager = GridManager.Instance;
        }

        public bool SetPosition(Vector2Int newGridPos)
        {
            if (!gridManager.HasTile(newGridPos))
                return false;
            
            gridPosition = newGridPos;
            transform.position = gridManager.tiles[gridPosition.x, gridPosition.y].transform.position;
            return true;
        }

        public bool Move(string direction)
        {
            // Try to move in the direction
            Vector2Int dir = Direction.ToVector2Int(direction);
            if (SetPosition(gridPosition + dir))
            {
                // Rotate in move direction
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                transform.eulerAngles = new Vector3(0, 0, angle);
                
                // It is a success
                return true;
            }
            
            // Failed to move
            return false;
        }
    }
}