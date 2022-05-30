using System;
using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using GridSystem;

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

        public int afraidCount;
        
        // Add and remove self automatically from the static enemies list
        protected virtual void Awake() => EnemyManager.enemies.Add(this);
        private void OnDestroy() => EnemyManager.enemies.Remove(this);

        protected override void Start()
        {
            base.Start();

            DefineStates();
            SetState(explorationState);
            Afraid();
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

        public void DoAction() {

           // currentState?.DoAction();

            Vector3 playerPos = PlayerManager.Instance.Player.transform.position;

            Vector3 playerToSightCenter = playerPos - (transform.position + transform.up * 0.5f);
 
            Vector3 sight = transform.up * 3;

            float angle = Vector2.Angle(playerToSightCenter, sight);


            if (angle < 90 && Vector2.Distance(playerPos, transform.position + transform.up * 0.5f) <= 3)
                Debug.Log("je vois le joueur");
 

        }
        
        public void Afraid()
        {
            afraidCount++;

            afraidCount = Mathf.Clamp(afraidCount, 0,3);

            if (afraidCount == 3) // A modifier le 3 avec le bravoure du Enemyprofile
                explorationState.directions = new Queue<Direction>(gridManager.GetPath(gridPosition, GameManager.Instance.levelEnd));
        }


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

        private void OnDrawGizmos() {
            Handles.color = new Color(Color.yellow.r,Color.yellow.g,Color.yellow.b,0.1f);
            Transform viewTransform = transform.GetChild(0);

            Handles.DrawSolidArc(viewTransform.position + transform.up * 0.5f, viewTransform.up, viewTransform.right,180,3); // 180 a changer par une valeur du so
        }

        
    }
}