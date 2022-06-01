using System;
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
        [SerializeField] private int defaultActionsPerTurn = 1;
        [SerializeField] private int transformedActionsPerTurn = 3;
        private int remainingActions;
        private int RemainingActions
        {
            get => remainingActions;
            set
            {
                remainingActions = value;
                if (remainingActions <= 0)
                {
                    // OnLaunchedTurn reset remainingAction
                    GameManager.Instance.LaunchTurn();
                    return;
                }

                Tile.ResetWalkableTiles();
                gridController.gridManager.SetNeighborTilesWalkable(gridController.currentTile, remainingActions);
            }
        }
        private void ResetRemainingAction(int phaseActionCount) => RemainingActions = transformed ? transformedActionsPerTurn : defaultActionsPerTurn;
        
        private bool hasPath;
        private List<Direction> path = new List<Direction>();
        [Header("Others"), SerializeField] 
        private int lightDamage;

        protected override void Start()
        {
            base.Start();
            gridController.OnMoved += OnMoved;

            cameraController = FindObjectOfType<CameraController>();
            
            RemainingActions = defaultActionsPerTurn;
            GameManager.Instance.OnLaunchedTurn += ResetRemainingAction;
        }

        private void Update()
        {
            if (hasPath)
                return;

            if (Input.touchCount > 0)
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Vector3 mousePosition = Vector3.zero;
                    // Do nothing if clicking UI
                    if (GameManager.eventSystem.IsPointerOverGameObject())
                        return;

                    if(PlayerManager.mapSeen == true)
                    {
                        Camera camMiniMap = PlayerManager.Instance.miniMap.GetComponent<Camera>();
                        mousePosition = camMiniMap.ScreenToWorldPoint(Input.mousePosition);
                    } else
                    {
                        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }


                    RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector3.right, 0.1f);
                    if (hits.Length == 0)
                        return;

                    // Look for IInteractable or Tile or... and break if found
                    foreach (var hit in hits)
                    {
                        if (hit.transform.TryGetComponent(out IInteractable interactable))
                        {
                            if (interactable.NeedToBeInRange)
                            {
                                if (gridController.gridManager.GetManhattanDistance(transform.position, hit.transform.position) <= 1 && interactable.IsInteractable)
                                {
                                    interactable.Interact();
                                    GameManager.Instance.LaunchTurn();
                                    break;
                                }
                            }
                            else
                            {
                                interactable.Interact();
                                break;
                            }
                        }
                        else if (hit.transform.TryGetComponent(out Tile tile))
                        {
                            if (gridController.gridPosition == tile.GridPos)
                                continue;

                            int manhattanDistance =
                                gridController.gridManager.GetManhattanDistance(gridController.gridPosition,
                                    tile.GridPos);
                            if (manhattanDistance <= RemainingActions)
                            {
                                gridController.SetPosition(tile.GridPos);
                                RemainingActions -= manhattanDistance;
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

            RemainingActions = transformedActionsPerTurn;
        }

        public void UnTransform()
        {
            if (!transformed)
                return;
            transformed = false;

            RemainingActions = defaultActionsPerTurn;
        }

        // private void StartPath(Vector2Int aimedGridPos)
        // {
        //     if (hasPath || moveCoroutine != null)
        //         return;
        //     path = gridController.gridManager.TempGetPath(gridController.gridPosition, aimedGridPos);
        //     hasPath = true;
        //
        //     moveCoroutine = MoveCoroutine();
        //     StartCoroutine(moveCoroutine);
        // }
        //
        // private IEnumerator moveCoroutine;
        //
        // IEnumerator MoveCoroutine()
        // {
        //     Tile.ResetWalkableTiles();
        //
        //     foreach (Direction direction in path)
        //     {
        //         gridController.Move(direction);
        //         yield return new WaitForSeconds(moveCooldown);
        //     }
        //
        //     OnStoppedMoving();
        //     hasPath = false;
        //     moveCoroutine = null;
        // }
        // private void OnStoppedMoving()
        // {
        //     gridController.gridManager.SetNeighborTilesWalkable(gridController.currentTile, RemainingActions);
        // }
        
        
        private void OnMoved(Vector2Int newGridPos)
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

            gridController.Move(Direction.FromVector2Int(vec));
            RemainingActions--;
        }

        #endregion
    }
}