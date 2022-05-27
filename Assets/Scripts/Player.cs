using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace GoldProject
{
    public class Player : Entity
    {
        public PlayerManager PlayerManager { private get; set; }
        private CameraController cameraController;

        [SerializeField] private float moveCooldown;
        private int movementRange = 3;
        private bool hasPath;
        private List<Direction> path = new List<Direction>();

        protected override void Start()
        {
            base.Start();
            cameraController = FindObjectOfType<CameraController>();
            gridManager.SetNeighborTilesWalkable(currentTile, movementRange);
        }

        private void Update()
        {
            if (hasPath)
                return;

            if (Input.touchCount > 0)
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    // Do nothing if clicking UI
                    if (GameManager.eventSystem.IsPointerOverGameObject())
                    {
                        return;
                    }
                    
                    Vector3 mousePosition = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);

                    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.right, 0.1f);

                    if (!hit)
                        return;
                    if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                    {
                        if (gridPosition == tile.GridPos)
                            return;
                            
                        if(gridManager.GetManhattanDistance(gridPosition, tile.GridPos) <= movementRange)
                            StartPath(tile.GridPos);
                    }
                }
        }

        private void StartPath(Vector2Int aimedGridPos)
        {
            if (hasPath || moveCoroutine != null)
                return;
            path = gridManager.TempGetPath(gridPosition, aimedGridPos);
            hasPath = true;

            moveCoroutine = MoveCoroutine();
            StartCoroutine(moveCoroutine);
        }

        private IEnumerator moveCoroutine;

        IEnumerator MoveCoroutine()
        {
            Tile.ResetWalkableTiles();
            
            foreach (Direction direction in path)
            {
                Move(direction);
                yield return new WaitForSeconds(moveCooldown);
            }

            OnStoppedMoving();
            hasPath = false;
            moveCoroutine = null;
        }


        protected override void OnMoved()
        {
            if (currentRoom.IsInGarlicRange(transform.position, out Garlic damagingGarlic))
            {
                Debug.Log("Garlic in range");
                PlayerManager.PlayerHealth.TakeDamage(damagingGarlic.damage);
            }
            
            GameManager.Instance.MoveAllEnemies();
        }

        private void OnStoppedMoving()
        {
            gridManager.SetNeighborTilesWalkable(currentTile, movementRange);
        }

        protected override void OnEnterRoom(Room room)
        {
            cameraController.ZoomToRoom(room);
        }

        #region UI Methods
        public void MoveLeft() => TryMove(Vector2Int.left);
        public void MoveRight() => TryMove(Vector2Int.right);
        public void MoveUp() => TryMove(Vector2Int.up);
        public void MoveDown() => TryMove(Vector2Int.down);
        public void TryMove(Vector2Int vec)
        {
            if (hasPath)
                return;

            Tile.ResetWalkableTiles();
            Move(Direction.FromVector2Int(vec));
            OnStoppedMoving();
        }
        #endregion
    }
}
