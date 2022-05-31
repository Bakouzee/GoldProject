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
        
        [Header("Movements")]
        [SerializeField] private float moveCooldown;
        [SerializeField] private int defaultMoveRange = 1;
        [SerializeField] private int transformedMoveRange = 3;
        private int currentMoveRange;
        private int CurrentMoveRange
        {
            get => currentMoveRange;
            set
            {
                currentMoveRange = value;
                Tile.ResetWalkableTiles();
                gridManager.SetNeighborTilesWalkable(currentTile, currentMoveRange);
            }
        }
        
        private bool hasPath;
        private List<Direction> path = new List<Direction>();
        [Header("Others"), SerializeField] 
        private int lightDamage;

        protected override void Start()
        {
            base.Start();

            cameraController = FindObjectOfType<CameraController>();
            
            CurrentMoveRange = defaultMoveRange;
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
                        return;

                    Vector3 mousePosition = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);

                    RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector3.right, 0.1f);
                    if (hits.Length == 0)
                        return;

                    // Look for IInteractable or Tile or... and break if found
                    foreach (var hit in hits)
                    {
                        if (hit.transform.TryGetComponent(out IInteractable interactable))
                        {
                            if (interactable.IsInteractable &&
                                gridManager.GetManhattanDistance(transform.position, hit.transform.position) <= 1)
                            {
                                interactable.Interact();
                                GameManager.Instance.LaunchTurn();
                                break;
                            }
                        }
                        else if (hit.transform.TryGetComponent(out Tile tile))
                        {
                            if (gridPosition == tile.GridPos)
                                continue;

                            if (gridManager.GetManhattanDistance(gridPosition, tile.GridPos) <= CurrentMoveRange)
                            {
                                // StartPath(tile.GridPos);
                                
                                Tile.ResetWalkableTiles();
                                SetPosition(tile.GridPos);
                                OnMoved();
                                GameManager.Instance.LaunchTurn();
                                gridManager.SetNeighborTilesWalkable(currentTile, CurrentMoveRange);
                                break;
                            }
                        }
                    }
                }
        }

        private bool transformed;

        public void Transform()
        {
            if (transformed)
                return;
            transformed = true;

            // TODO: Waiting for Frighten method in enemies
            // Frighten enemies in the room
            // foreach (var enemy in currentRoom.enemies)
            // enemy.Frighten();

            CurrentMoveRange = transformedMoveRange;
        }

        public void UnTransform()
        {
            if (!transformed)
                return;
            transformed = false;

            CurrentMoveRange = defaultMoveRange;
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

            if (currentRoom.IsInLight(transform.position))
            {
                Debug.Log("Take damage from light");
                PlayerManager.PlayerHealth.TakeDamage(lightDamage);
            }
            
            // GameManager.Instance.LaunchTurn();
        }

        private void OnStoppedMoving()
        {
            gridManager.SetNeighborTilesWalkable(currentTile, CurrentMoveRange);
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