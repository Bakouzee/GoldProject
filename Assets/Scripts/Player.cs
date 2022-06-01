﻿using System;
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
        
        [Header("Others"), SerializeField] 
        private int lightDamage;

        protected override void Start()
        {
            GameManager gameManager = GameManager.Instance;
            
            base.Start();
            gridController.OnMoved += OnMoved;

            cameraController = FindObjectOfType<CameraController>();

            // Transform or Untransform on day or night start
            gameManager.OnDayStart += UnTransform;
            gameManager.OnNightStart += Transform;
            
            RemainingActions = defaultActionsPerTurn;
            // When a turn is launched -> reset the number of remaining action
            gameManager.OnLaunchedTurn += ResetRemainingAction;
            
            // Get the ability to transform if a chief leave or die
            Enemies.EnemyManager.OnEnemyDisappeared += enemy =>
            {
                if (enemy.chief)
                    canTransform = true;
            };
        }

        private void Update()
        {
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


                    RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector3.back, 0.1f);
                    if (hits.Length == 0)
                        return;

                    // Look for IInteractable or Tile or... and break if found
                    foreach (var hit in hits)
                    {
                        // Interactable objects
                        if (hit.transform.TryGetComponent(out IInteractable interactable))
                        {
                            // Just to not copy/paste
                            System.Action interact = () => { interactable.Interact(); GameManager.Instance.LaunchTurn(); };
                            
                            if (interactable.NeedToBeInRange)
                            {
                                if (gridController.gridManager.GetManhattanDistance(transform.position, hit.transform.position) <= 1 && interactable.IsInteractable)
                                {
                                    interact.Invoke();
                                    break;
                                }
                            }
                            else
                            {
                                interact.Invoke();
                                break;
                            }
                        }
                        
                        // Tiles
                        else if (hit.transform.TryGetComponent(out Tile tile))
                        {
                            if (gridController.gridPosition == tile.GridPos)
                                continue;

                            int manhattanDistance =
                                gridController.gridManager.GetManhattanDistance(gridController.gridPosition,
                                    tile.GridPos);
                            if (manhattanDistance <= RemainingActions)
                            {
                                if (gridController.SetPosition(tile.GridPos))
                                {
                                    RemainingActions -= manhattanDistance;
                                    break;
                                }
                            }
                        }
                    }
                }
        }

        #region Transformation
        public static bool transformed;
        private bool canTransform;
        public void Transform()
        {
            if (transformed || !canTransform)
                return;
            canTransform = false;
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
        #endregion

        private void OnMoved(Vector2Int newGridPos)
        {
            if (currentRoom.IsInGarlicRange(transform.position, out Garlic damagingGarlic))
            {
                // Take damage from garlic
                PlayerManager.PlayerHealth.TakeStinkDamage(damagingGarlic.damage);
            }

            if (currentRoom.IsInLight(transform.position))
            {
                // Take damage from light
                PlayerManager.PlayerHealth.TakeFireDamage(lightDamage);
            }
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
            if(gridController.Move(Direction.FromVector2Int(vec)))
                // Only decrement remaining action if Move is a success
                RemainingActions--;
        }

        #endregion
    }
}