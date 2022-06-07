using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;

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
        public bool canBeAfraid;

        
        [Space(20)]
        [Tooltip("Windows, vents, etc... will not be detected if too far")]
        [SerializeField] private int objectDetectionRange;
        
        [Header("Window")]
        [Tooltip("Probabilty of opening a windowwhen passing next to a closed window")]
        [Range(0f, 1f)] public float openWindowProba;
        
        [Header("Garlic")]
        [Tooltip("Probability of putting a garlic on the floor when passing next to an open window")]
        [Range(0f, 1f)] public float garlicProba;
        public Garlic garlicPrefab; 
        
        protected Health health;
        
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
        public bool isInSight;
        
        
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

            stateColor = Color.yellow;
            stateColor.a = 0.1f;

            gridController.OnMoved += OnMoved;
        }
        
        /// <summary>
        /// Method only used to define each EnemyBaseState
        /// and called by default in the start
        /// </summary>
        protected virtual void DefineStates() =>  explorationState = new ExplorationStateBase(this);

        

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
            

            if (isInSight || isAlerted) { // Quand un ennemi spawn après qu'il y a eu l'alerte il le chase qd mm          
                currentRoom.enemies.ForEach(delegate (EnemyBase enemy) {
                    if (!(enemy.currentState is ChaseState))  {
                        this.gameObject.name = "Chief Of Patrol";
                        enemy.SetState(new ChaseState(enemy, PlayerManager.Instance.Player,this));
                        enemy.stateColor = Color.red;
                        enemy.stateColor.a = 0.1f;
                        enemy.isAlerted = true;
                    }
                });
  
            }

            if(currentState is ChaseState) {
                ChaseState chase = (ChaseState)currentState;
                Debug.Log(chase.chief.currentRoom.name);
                if(!chase.chief.isInSight) {
                    chase.chief.currentRoom.enemies.ForEach(delegate (EnemyBase enemy) {
                        enemy.SetState(new ExplorationStateBase(enemy));
                        enemy.stateColor = Color.yellow;
                        enemy.stateColor.a = 0.1f;
                        enemy.isAlerted = false;
                    });
                }
            }
            

            currentState?.DoAction();
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

            Debug.Log("The enemy is afraid !");
        }
        
        
        // IInteractable implementation
        public Transform Transform => transform;
        public bool IsInteractable => Player.transformed;
        public bool NeedToBeInRange => true;
        public void Interact()
        {
            if (health.TakeDamage(1))
            {
                // If died -> call OnEnemyKilled event
                EnemyManager.OnEnemyKilled?.Invoke(this);
            }
        }

        private void OnMoved(Vector2Int newGridPos)
        {
            if(GridManager.Instance.GetManhattanDistance(newGridPos,PlayerManager.Instance.Player.gridController.gridPosition) <= 1)     
                PlayerManager.Instance.PlayerHealth.Death();
            
        }

        private void OnDrawGizmos() {
            //Handles.color = stateColor;
            Transform viewTransform = transform.GetChild(0);

            //Handles.DrawSolidArc(viewTransform.position + transform.up * 0.5f, viewTransform.up, viewTransform.right,sightAngle * 2,sightRange); 
        }

        
    }
}