using System;
using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies
{
    /// <summary>
    /// Base class of an enemy
    /// Is designed to be inherited to create different
    /// types of enemy
    /// </summary>
    public class EnemyBase : Entity, IInteractable
    {
        /// <summary>
        /// Is the enemy the chief of exploration
        /// </summary>
        public bool chief;
        public bool canBeAfraid;
        
        protected Health health;
        
        // States
        protected EnemyBaseState currentState;
        protected ExplorationStateBase explorationState;
        public GridController GridController => gridController;

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
            
            DefineStates();
            SetState(explorationState);
        }
        
        /// <summary>
        /// Method only used to define each EnemyBaseState
        /// and called by default in the start
        /// </summary>
        protected virtual void DefineStates()
        {
            explorationState = new ExplorationStateBase(this);
        }
        
        protected virtual void Update() => currentState?.OnStateUpdate();
        
        /// <summary>
        /// Do Action method, let the current state choose the action to do
        /// This method is called by the GameManager in every enemy at each turn
        /// </summary>
        public void DoAction() => currentState?.DoAction();

        
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
                new RunningState(
                    enemy: this,
                    frighteningSource: source,
                    numberOfTurn: 3,
                    nextState: new ExplorationStateBase(this)
                )
            );
        }
        
        
        // IInteractable implementation
        public bool IsInteractable => Player.transformed;
        public bool NeedToBeInRange => true;
        public void Interact()
        {
            health.TakeDamage(1);
        }
    }
}