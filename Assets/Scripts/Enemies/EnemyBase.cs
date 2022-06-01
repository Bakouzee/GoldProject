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
        public GridController GridController => gridController;

        public int afraidToLeave;
        private int afraidCount;

        [Range(0,90)]
        public int sightAngle;

        [Range(0,10)]
        public int sightRange;

        public Color stateColor;

        public bool isAlerted;
        
        
        // Add and remove self automatically from the static enemies list
        protected virtual void Awake() => EnemyManager.enemies.Add(this);
        private void OnDestroy() => EnemyManager.enemies.Remove(this);

        protected override void Start()
        {
            base.Start();

            DefineStates();
            SetState(explorationState);

            stateColor = Color.yellow;
            stateColor.a = 0.1f;
        }
        
        /// <summary>
        /// Method only used to define each EnemyBaseState
        /// and called by default in the start
        /// </summary>
        protected virtual void DefineStates()
        {
            explorationState = new ExplorationStateBase(this);

        }

        protected virtual void Update()  {
            currentState?.OnStateUpdate();

            if (gridController.gridPosition == GameManager.Instance.tileEnd)
                Destroy(transform.gameObject);
            
        }

        /// <summary>
        /// Do Action method, let the current state choose the action to do
        /// This method is called by the GameManager in every enemy at each turn
        /// </summary>

        public void DoAction() {

            

            Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector3 playerToSightCenter = playerPos - (transform.position + transform.up * 0.5f);
            Vector3 sight = transform.up * sightRange;

            float angle = Vector2.Angle(playerToSightCenter, sight);


            bool isInSight = angle < sightAngle && Vector2.Distance(playerPos, transform.position + transform.up * 0.5f) <= sightRange;

        /*    Debug.Log("=========================");
            Debug.Log("name " + name);
            Debug.Log("isAlerted " + isAlerted);
            Debug.Log("isInSight " + isInSight);
            */

            if (isInSight || isAlerted) {              
                if(!(currentState is ChaseState)) {
                    SetState(new ChaseState(this, PlayerManager.Instance.Player));
                    stateColor = Color.red;
                    stateColor.a = 0.1f;
                }

                
            }
            else {
                if(currentState is ChaseState) {
                    
                    SetState(explorationState);
                    stateColor = Color.yellow;
                    stateColor.a = 0.1f;

                }
            }

            currentState?.DoAction();
        }
        
        public void Afraid()
        {
            afraidCount++;

            afraidCount = Mathf.Clamp(afraidCount, 0,3);

            if (afraidCount == afraidToLeave) 
                explorationState.directions = new Queue<Direction>(GridManager.Instance.GetPath(GridManager.Instance.GetGridPosition(transform.position),GameManager.Instance.tileEnd));
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
            Handles.color = stateColor;
            Transform viewTransform = transform.GetChild(0);

            Handles.DrawSolidArc(viewTransform.position + transform.up * 0.5f, viewTransform.up, viewTransform.right,sightAngle * 2,sightRange); 
        }

        
    }
}