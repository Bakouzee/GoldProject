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

        public bool SetPosition(Vector3 worldPosition) => SetPosition(gridManager.GetGridPosition(worldPosition));
        public bool SetPosition(Vector2Int newGridPos)
        {
            if (!gridManager.HasTile(newGridPos))
                return false;
            
            gridPosition = newGridPos;
            transform.position = (Vector2)gridManager.GetTileAtPosition(gridPosition).transform.position;
            
            OnMoved?.Invoke(gridPosition);
            return true;
        }


        public bool Move(Direction direction, Animator animator = null) => Move(direction.Value, animator);
        public bool Move(string direction, Animator animator = null) =>
            Move(Direction.ToVector2Int(direction), animator);
        public bool Move(Vector2Int dir, Animator animator = null)
        {
            if (SetPosition(gridPosition + dir))
            {
                // Play animation in direction
                if(animator) animator.SetTrigger(Direction.FromVector2Int(dir).ToString());
                
                //transform.eulerAngles = new Vector3(0, 0, angle);
                
                // It is a success
                return true;
            }
            
            // Failed to move
            return false;
        }

        public System.Action<Vector2Int> OnMoved;
    }
}