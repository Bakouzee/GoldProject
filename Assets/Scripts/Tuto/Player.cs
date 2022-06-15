﻿using System;
using AudioController;
using Enemies;
using GridSystem;
using UnityEngine;
using GoldProject;
using GoldProject.Rooms;
using SplashArt;

namespace Tuto
{
        public class Player : Entity
    {
        public PlayerManager PlayerManager { private get; set; }
        private CameraController cameraController;
        [SerializeField] private Animator animator;

        [Header("Actions")] [SerializeField] private int defaultActionsPerTurn = 1;
        [SerializeField] private int transformedActionsPerTurn = 3;
        private int remainingActions;

        private int RemainingActions
        {
            get => remainingActions;
            set
            {
                if (value <= 0)
                {
                    ResetRemainingAction();
                    return;
                }

                // Only if we want to damage enemy on each move
                // else if(remainingActions > value)
                // {
                //     LookForGarlicDamage();
                //     LookForLightDamage();
                // }
                remainingActions = value;

                Tile.ResetWalkableTiles();
                gridController.gridManager.SetNeighborTilesWalkable(gridController.currentTile, remainingActions);
            }
        }

        private void ResetRemainingAction() =>
            RemainingActions = (transformed ? transformedActionsPerTurn : defaultActionsPerTurn) +
                               PlayerManager.Bonuses.GetBonusesOfType(Bonus.Type.ActionPerTurn);

        private int interactionRange => 1 + PlayerManager.Bonuses.GetBonusesOfType(Bonus.Type.InteractionRange);

        [Header("Others"), SerializeField] private int lightDamage;
        [SerializeField] private int lifeStealOnKill;
        [SerializeField] private int frightenRadiusOnKill = 5;

        protected override void Start()
        {
            cameraController = FindObjectOfType<CameraController>();
            GameManager gameManager = GameManager.Instance;

            base.Start();
            gridController.OnMoved += OnMoved;

            RemainingActions = defaultActionsPerTurn;
            SetEnemyManagerEvents();
        }

        #region Set Events
        private void SetEnemyManagerEvents()
        {
            // Get the ability to transform if a chief leave or die
            EnemyManager.OnEnemyDisappeared += enemy =>
            {
                // Debug.Log("Enemy disappear");
                if (enemy.chief)
                    canTransform = true;
            };

            // When killing an enemy
            EnemyManager.OnEnemyKilled += enemy =>
            {
                // Art when the enemy killed is the chief
                if (enemy.chief)
                    SplashArtManager.Instance.SplashArtToChoose(SplashArtType.Transformation_First_Kill);

                // Heal when killing an enemy
                PlayerManager.PlayerHealth.HealPlayer(lifeStealOnKill);

                // Fear all nearby enemies
                foreach (var enemyBase in currentRoom.enemies)
                {
                    if (gridController.gridManager.GetManhattanDistance(transform.position,
                        enemyBase.transform.position) <= frightenRadiusOnKill)
                        enemyBase.GetAfraid(transform);
                }
            };
        }

        #endregion

        private void Update()
        {
            // TEST AUDIO FOOTSTEP
            if (Input.GetKeyDown(KeyCode.E))
            {
                AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Footstep);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                canTransform = true;
                Transform();
            }

            if (Input.touchCount > 0)
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Vector3 mousePosition = Vector3.zero;
                    // Do nothing if clicking UI
                    if (GameManager.eventSystem.IsPointerOverGameObject())
                        return;

                    if (PlayerManager.mapSeen == true)
                    {
                        Camera camMiniMap = PlayerManager.Instance.miniMap.GetComponent<Camera>();
                        mousePosition = camMiniMap.ScreenToWorldPoint(Input.mousePosition);
                    }
                    else
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
                            if (interactable.NeedToBeInRange)
                            {
                                if (gridController.gridManager.GetManhattanDistance(transform.position,
                                    hit.transform.position) <= interactionRange && interactable.IsInteractable)
                                {
                                    if (interactable.TryInteract())
                                    {
                                        // GameManager.Instance.LaunchTurn();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (interactable.TryInteract())
                                {
                                    // GameManager.Instance.LaunchTurn();
                                    break;
                                }
                            }
                        }

                        // Tiles and if map = cantmove
                        else if (hit.transform.TryGetComponent(out Tile tile) && !PlayerManager.mapSeen &&
                                 !NewVentManager.choosingVent)
                        {
                            if (gridController.gridPosition == tile.GridPos)
                                continue;

                            int manhattanDistance =
                                gridController.gridManager.GetManhattanDistance(gridController.gridPosition,
                                    tile.GridPos);
                            if (manhattanDistance <= RemainingActions)
                            {
                                if (gridController.SetPosition(tile.GridPos, animator))
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
        private bool CanTransform
        {
            get => canTransform;
            set
            {
                canTransform = value;
                OnCanTransformChanged?.Invoke(canTransform);
            }
        }
        public System.Action<bool> OnCanTransformChanged;

        public void Transform()
        {
            if (transformed || !canTransform)
                return;
            canTransform = false;
            transformed = true;

            SplashArtManager.Instance.SplashArtToChoose(SplashArtType.Transformation);
            AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Transformation);

            // Changer current animation layer
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
            
            // Frighten enemies in the room
            foreach (var enemy in currentRoom.enemies)
                enemy.GetAfraid(source: transform);

            RemainingActions = transformedActionsPerTurn;
        }

        public void UnTransform()
        {
            if (!transformed)
                return;
            transformed = false;

            SplashArtManager.Instance.SplashArtToChoose(SplashArtType.Untransformation);

            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0);

            RemainingActions = defaultActionsPerTurn;
        }

        #endregion

        private void OnMoved(Direction direction)
        {
            if (!currentRoom.IsInside(transform.position))
                UpdateCurrentRoom();
            
            LookForGarlicDamage();
            LookForLightDamage();
            // RemainingActions--;
        }

        private void LookForGarlicDamage()
        {
            if (currentRoom.IsInGarlicRange(transform.position, out Garlic damagingGarlic))
            {
                // Take damage from garlic
                PlayerManager.PlayerHealth.TakeStinkDamage(damagingGarlic.damage);
            }
        }

        public void LookForLightDamage()
        {
            if (currentRoom.IsInLight(transform.position))
            {
                // Take damage from light
                PlayerManager.PlayerHealth.TakeFireDamage(lightDamage);
            }
        }

        protected override void UpdateCurrentRoom()
        {
            var lastRoom = currentRoom;
            base.UpdateCurrentRoom();

            if (currentRoom != lastRoom) //&& !cameraController.dezoomCam)
            {
                cameraController.ZoomToRoom(currentRoom);
            }
        }

        protected override void OnEnterRoom(Room room)
        {
            //if(!cameraController.dezoomCam)
            cameraController.ZoomToRoom(room);
        }

        #region UI Methods

        public void MoveLeft() => TryMove(Vector2Int.left);
        public void MoveRight() => TryMove(Vector2Int.right);
        public void MoveUp() => TryMove(Vector2Int.up);
        public void MoveDown() => TryMove(Vector2Int.down);

        public void TryMove(Vector2Int vec)
        {
            if (gridController.Move(Direction.FromVector2Int(vec), animator))
                // Only decrement remaining action if Move is a success
                RemainingActions--;
        }

        #endregion
    }
}