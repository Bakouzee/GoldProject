using System;
using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
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
        // States
        protected EnemyBaseState currentState;
        protected ExplorationStateBase explorationState;



        
        // Add and remove self automatically from the static enemies list
        protected virtual void Awake() => EnemyManager.enemies.Add(this);
        private void OnDestroy() => EnemyManager.enemies.Remove(this);

        protected override void Start()
        {   
            base.Start();

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
        protected virtual void SetState(EnemyBaseState enemyBaseState)
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

        // IInteractable implementation
        public bool IsInteractable => GameManager.dayState == GameManager.DayState.NIGHT;
        public void Interact()
        {
            // Take Damage
        }
    }
}