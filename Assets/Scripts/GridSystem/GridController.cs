using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace GridSystem
{
    public class GridController
    {
        protected Transform transform;
        public GridManager gridManager;
        public Vector2Int gridPosition;
        public Tile currentTile => gridManager.GetTileAtPosition(gridPosition);

        public int speed;

        public GridController(Transform transform)
        {
            this.transform = transform;
            this.gridManager = GridManager.Instance;
            gridPosition = gridManager.GetGridPosition(transform.position);
            SetPosition(gridPosition);
        }
        
        public bool SetPosition(Vector2Int newGridPos)
        {
            if (!gridManager.HasTile(newGridPos))
                return false;
            
            gridPosition = newGridPos;
            transform.position = (Vector2)gridManager.GetTileAtPosition(gridPosition).transform.position;
            return true;
        }


        public bool Move(Direction direction) => Move(direction.Value);
        public bool Move(string direction)
        {
            // Try to move in the direction
            Vector2Int dir = Direction.ToVector2Int(direction);

            if (SetPosition(gridPosition + dir))
            {
                // Rotate in move direction
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
                transform.eulerAngles = new Vector3(0, 0, angle);
                
                OnMoved?.Invoke(gridPosition);
                
                // It is a success
                return true;
            }
            
            // Failed to move
            return false;
        }

        public System.Action<Vector2Int> OnMoved;
    }
}