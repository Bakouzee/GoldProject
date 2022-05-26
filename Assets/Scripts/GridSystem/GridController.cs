using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace GridSystem
{
    public abstract class GridController : MonoBehaviour
    {
        private GridManager gridManager;
        private Vector2Int gridPosition;

        public int speed;

        protected virtual void Start()
        {
            gridManager = GridManager.Instance;
            gridPosition = new Vector2Int((int)transform.position.x, (int) transform.position.y);
        }

        public bool SetPosition(/*Vector2Int newGridPos*/ Vector2Int direction)
        {
            transform.gameObject.GetComponent<Rigidbody2D>().velocity = direction * speed;
            return true;
        }

        public bool Move(string direction)
        {
            // Try to move in the direction
            Vector2Int dir = Direction.ToVector2Int(direction);

            if (SetPosition(dir))
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