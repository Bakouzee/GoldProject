using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace GridSystem
{
    public abstract class GridController : MonoBehaviour
    {
        protected GridManager gridManager;
        protected Vector2Int gridPosition;
        protected Tile currentTile => gridManager.GetTileAtPosition(gridPosition);

        public int speed;

        protected virtual void Start()
        {
            gridManager = GridManager.Instance;
            gridPosition = new Vector2Int((int)transform.position.x, (int) transform.position.y);
        }

        public bool SetVelocity(Vector2Int direction)
        {
            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = direction * speed;
                return true;
            }
            return false;
        }
        public bool SetPosition(Vector2Int newGridPos)
        {
            if (!gridManager.HasTile(newGridPos))
                return false;
            
            gridPosition = newGridPos;
            transform.position = gridManager.GetTileAtPosition(gridPosition).transform.position;
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
                
                OnMoved();
                
                // It is a success
                return true;
            }
            
            // Failed to move
            return false;
        }

        protected virtual void OnMoved()
        {
            
        }
    }
}