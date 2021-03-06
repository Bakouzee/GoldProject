using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using GoldProject.Rooms;
using GoldProject.UI;
using GridSystem;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using AudioController;
using SplashArt;

namespace GoldProject
{
    public class Player : Entity
    {
        public PlayerManager PlayerManager { private get; set; }
        private CameraController cameraController;
        [SerializeField] private Animator animator;

        [Header("Actions")] [SerializeField] private int defaultActionsPerTurn = 1;
        [SerializeField] private int transformedActionsPerTurn = 3;
        private int remainingActions;

        public ParticleSystem bloodParticule;

        private int RemainingActions
        {
            get => remainingActions;
            set
            {
                if (value <= 0)
                {
                    // OnLaunchedTurn reset remainingAction
                    GameManager.Instance.LaunchTurn();
                    return;
                }

                // Only if we want to damage enemy on each move
                // else if(remainingActions > value)
                // {
                //     LookForGarlicDamage();
                //     LookForLightDamage();
                // }
                remainingActions = value;
                SetNeighborTilesWalkable();
            }
        }

        public void SetNeighborTilesWalkable()
        {
            Tile.ResetWalkableTiles();
            gridController.gridManager.SetNeighborTilesWalkable(gridController.currentTile, remainingActions);
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
            SetGameHandlerEvents(gameManager);
            SetEnemyManagerEvents();

            CanTransform = false;
            transformed = false;
        }

        #region Set Events

        private void SetGameHandlerEvents(GameManager gameManager)
        {
            // Transform or Untransform on day or night start
            gameManager.OnDayStart += UnTransform;
            gameManager.OnNightStart += Transform;

            // When a turn is launched -> reset the number of remaining action
            gameManager.OnLaunchedTurn += (phaseActionCount, actionPerPhase) =>
            {
                LookForGarlicDamage();
                LookForLightDamage();
                ResetRemainingAction();
            };
        }

        private void SetEnemyManagerEvents()
        {
            // Get the ability to transform if a chief leave or die
            EnemyManager.OnEnemyDisappeared += enemy =>
            {
                Debug.Log("Enemy disappear");
                if (!enemy.chief || enemy.Leaving)
                    return;
                CanTransform = true;
            };

            // When killing an enemy
            EnemyManager.OnEnemyKilled += enemy =>
            {
                bloodParticule.Play();
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
                CanTransform = true;
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

                    // Look for IInteractable and return if found
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
                                        GameManager.Instance.LaunchTurn();
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (interactable.TryInteract())
                                {
                                    GameManager.Instance.LaunchTurn();
                                    return;
                                }
                            }
                        }
                    }

                    // Look for Tiles and return if found 
                    if (!PlayerManager.mapSeen && !NewVentManager.choosingVent)
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.transform.TryGetComponent(out Tile tile))
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
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
        }

        #region Transformation

        public static bool transformed;

        private bool canTransform;

        public bool CanTransform
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
            if (transformed || !CanTransform)
                return;
            CanTransform = false;
            transformed = true;

            // Play particles when transformation
            if (UiManager.Instance.transformationParticles)
            {
                UiManager.Instance.transformationParticles.gameObject.SetActive(true);
                UiManager.Instance.transformationParticles.Play();
            }


            // Play splash art when turning
            SplashArtManager.Instance.SplashArtToChoose(SplashArtType.Transformation);

            // Play audio
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

            // Stop particles when untransformation
            if (UiManager.Instance.transformationParticles)
            {
                UiManager.Instance.transformationParticles.gameObject.SetActive(false);
            }

            SplashArtManager.Instance.SplashArtToChoose(SplashArtType.Untransformation);
            AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Detransformation);

            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0);

            RemainingActions = defaultActionsPerTurn;
        }

        #endregion

        private void OnMoved(Direction direction)
        {
            if (!currentRoom.IsInside(transform.position))
                UpdateCurrentRoom();
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

        public ColorCurves cruves;
        public Gradient gradient;
    }
}