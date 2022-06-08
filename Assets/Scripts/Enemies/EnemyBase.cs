﻿using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using Enemies;
using System.Collections.Generic;
using AudioController;

namespace Enemies
{
    /// <summary>
    /// Base class of an enemy
    /// Is designed to be inherited to create different
    /// types of enemy
    /// </summary>
    public class EnemyBase : Entity, IInteractable
    {
        /// <summary>Is the enemy the chief of exploration</summary>
        public bool chief;
        /// <summary>Is sensible to frightening traps</summary>
        [SerializeField] bool canBeAfraid;
        /// <summary>Is sensible to attracting traps</summary>
        [SerializeField] private bool canBeAttracted;

        
        [Header("Window")]
        [Tooltip("Probabilty of opening a windowwhen passing next to a closed window")]
        [Range(0f, 1f)] public float openWindowProba;
        [SerializeField] private int closedWindowDetectionRange;
        
        [Header("Garlic")]
        [Tooltip("Probability of putting a garlic on the floor when passing next to an open window")]
        public int detectionRangeForGarlic;
        [Range(0f, 1f)] public float garlicProba;
        public Garlic garlicPrefab;
        public bool HasPlacedGarlic { get; set; }
        
        protected Health health;
        
        // States
        protected EnemyBaseState currentState;
        protected EnemyBaseState lastState;
        public GridController GridController => gridController;

        [Header("Afraid vars")]
        public int afraidToLeave;
        private int afraidCount;

        [Header("Player Detection")]
        [Range(0,90)]
        public int sightAngle;

        [Range(0,10)]
        public int sightRange;

        public int curtainRange;
        [Range(0,100)]
        public int curtainProbability;


        public Color stateColor;

        public bool isAlerted;
        public bool isInSight;
        public bool canSightPlayer;
        private bool lastIsInSight;

        private List<EnemyBase> roomEnemies;
        public Animator animator;

        // Add and remove self automatically from the static enemies list
        protected virtual void Awake() => EnemyManager.enemies.Add(this);
        private void OnDestroy()
        {
            // remove self from room enemies list
            if (currentRoom != null)
                currentRoom.enemies.Remove(this);
            
            // Remove self from allEnemies list
            EnemyManager.enemies.Remove(this);
        }

        protected override void Start()
        {
            base.Start();

            health = GetComponent<Health>();
            
            // Call EnemyManager.OnEnemyDeath when dead
            health.OnDeath += () =>
            {
                EnemyManager.OnEnemyDeath?.Invoke(this);
                // foreach (var enemy in currentRoom.enemies)
                // {
                //     enemy.Afraid();
                // }
            };
            
            SetState(new ExplorationStateBase(this));

            stateColor = Color.yellow;
            stateColor.a = 0.1f;

            gridController.OnMoved += OnMoved;
        }
        

        protected virtual void Update() => currentState?.OnStateUpdate();
            
        
        /// <summary>
        /// Do Action method, let the current state choose the action to do
        /// This method is called by the GameManager in every enemy at each turn
        /// </summary>

         
        public void DoAction() {      
            Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector3 playerToSightCenter = playerPos - (transform.position + transform.up * 0.5f);
            Vector3 sight = transform.up * sightRange;

            float angle = Vector2.Angle(playerToSightCenter, sight);


            isInSight = angle < sightAngle && Vector2.Distance(playerPos, transform.position + transform.up * 0.5f) <= sightRange;

            if (isInSight && !lastIsInSight) {
                Debug.Log("enter vision");
                roomEnemies = currentRoom.enemies;
            }


            if ((isInSight || isAlerted) && canSightPlayer) { // Quand un ennemi spawn après qu'il y a eu l'alerte il le chase qd mm          
                roomEnemies.ForEach(delegate (EnemyBase enemy) {
                    if (!(enemy.currentState is EnemyChaseState))  {
                        this.gameObject.name = "Chief Of Patrol";
                        enemy.SetState(new EnemyChaseState(enemy, PlayerManager.Instance.Player,this));
                        enemy.stateColor = Color.red;
                        enemy.stateColor.a = 0.1f;
                        enemy.isAlerted = true;
                        AudioManager.Instance.PlayEnemySound(EnemyAudioTracks.E_Trigger);
                    }
                });
  
            }

            if(currentState is EnemyChaseState) {
                EnemyChaseState enemyChase = (EnemyChaseState)currentState;    
                if(!enemyChase.chief.isInSight) {
                    enemyChase.chief.currentRoom.enemies.ForEach(delegate (EnemyBase enemy) {
                        if(enemy.isAlerted && enemy.canSightPlayer) {
                            enemy.SetState(new ExplorationStateBase(enemy));
                            enemy.stateColor = Color.yellow;
                            enemy.stateColor.a = 0.1f;
                            enemy.isAlerted = false;
                        }
                    });
                }
            }
            

            currentState?.DoAction();

            lastIsInSight = isInSight;
        }

        /// <summary>
        /// Method to change the current state
        /// It handles the exit of the current state
        /// and the enter in the new one
        /// </summary>
        /// <param name="enemyBaseState"></param>
        public virtual void SetState(EnemyBaseState enemyBaseState)
        {
            if (enemyBaseState == null)
                return;
            if(currentState != null) StartCoroutine(currentState.OnStateExit());
            lastState = currentState;
            currentState = enemyBaseState;
            StartCoroutine(currentState.OnStateEnter());
        }

        // Add or Remove self to room enemie list when entering or exiting
        protected override void OnExitRoom(Room room) => room.enemies.Remove(this);
        protected override void OnEnterRoom(Room room) => room.enemies.Add(this);


        public void GetAfraid(Transform source)
        {
            if (!canBeAfraid)
                return;
            
            SetState(
                new EnemyAfraidState(
                    enemy: this,
                    frighteningSource: source,
                    numberOfTurn: 3,
                    nextState: new ExplorationStateBase(this)
                )
            );

            AudioManager.Instance.PlayEnemySound(EnemyAudioTracks.E_Fear);
            Debug.Log("The enemy is afraid !");
        }

        public void GetAttracted(Vector2Int attractionGridPos, System.Action onArrived)
        {
            if (!canBeAttracted)
                return;
            
            SetState(new EnemyGoToState(
                enemy: this, 
                aimedGridPos: attractionGridPos,
                onArrived: onArrived, 
                nextState: new ExplorationStateBase(this)
                )
            );
        }
        
        
        // IInteractable implementation
        public Transform Transform => transform;
        public bool IsInteractable => Player.transformed;
        public bool NeedToBeInRange => true;
        public bool TryInteract()
        {
            if (health.TakeDamage(1))
            {
                // If died -> call OnEnemyKilled event
                EnemyManager.OnEnemyKilled?.Invoke(this);
            }
            return true;
        }

        private void OnMoved(Vector2Int newGridPos)
        {
            Curtain closest = currentRoom.GetClosestCurtain(transform.position);
            
            if(closest != null && GridManager.Instance.GetManhattanDistance(gridController.gridPosition, new Vector2Int((int)closest.transform.position.x, (int)closest.transform.position.y)) <= curtainRange 
                && !(currentState is EnemyInteractState) && !closest.IsOpened)    {
                int random = Random.Range(0, 100);

                if (random <= curtainProbability)            
                    SetState(new EnemyInteractState(this, new ExplorationStateBase(this), closest));
                
            }
            
           /* VentManager vent = currentRoom.GetClosestVent(transform.position);
            vent.GetComponent<SpriteRenderer>().color = Color.green;

            Vector2Int ventPos = new Vector2Int((int)vent.transform.position.x, (int)vent.transform.position.y);

            if (GridManager.Instance.GetManhattanDistance(gridController.gridPosition, ventPos) <= garlicRange && !(currentState is RunningState))
            {
                int random = Random.Range(0, 100);
                Debug.Log(ventPos);
                Debug.Log(garlicRange);
                if (random <= garlicProbability)
                    SetState(new RunningState(enemy: this,frighteningSource: vent.transform,numberOfTurn: garlicRange,nextState: new ExplorationStateBase(this),() =>
                    {
                        //Debug.Log("end state");
                    }));
            }

    */
        }

        private void OnDrawGizmos() {
            //Handles.color = stateColor;
            Transform viewTransform = transform.GetChild(0);

            //Handles.DrawSolidArc(viewTransform.position + transform.up * 0.5f, viewTransform.up, viewTransform.right,sightAngle * 2,sightRange); 
        }

        
    }
}