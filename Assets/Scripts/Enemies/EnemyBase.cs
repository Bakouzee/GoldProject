using Enemies.States;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
        public GridController GridController => gridController;
        
        /// <summary>Is the enemy the chief of exploration</summary>
        public bool chief;
        /// <summary>Is sensible to frightening traps</summary>
        [SerializeField] bool canBeAfraid;
        /// <summary>Is sensible to attracting traps</summary>
        [SerializeField] bool canBeAttracted;

        
        [Header("Window")]
        [Tooltip("Probabilty of opening a window when passing next to a closed window")]
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
        public EnemyBaseState CurrentState => currentState;
        /// <summary>This variable is just used so we can see the current state
        /// in the inspector while in debug mode</summary>
        private string currentStateName;
        protected EnemyBaseState lastState;

        [Header("Afraid vars")]
        public ParticleSystem tearsParticules;
        public int bravery = 1;
        private int afraidCount;

        [Header("Player Detection")]
        [Range(0,180)]
        public int sightAngle = 90;
        [Range(0,10)]
        public int sightRange;
        public GameObject exclamationPoint;
        public GameObject interrogationPoint;

        [Header("Open Curtains var")]
        public int curtainRange;
        [Range(0,100)]
        public int curtainProbability;


        private Color stateColor;

        public bool isAlerted;
        public bool isInSight;
        public bool canSightPlayer;
        public Vector2Int lastPlayerPos;

        [Header("Detection Light")]
        [SerializeField] private Light2D detectionSpotlight;
        [SerializeField] private Color defaultLightColor;
        [SerializeField] private Color chaseLightColor;

        [Header("Others")]
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        private Vector2Int lastMoveDirection;
        public bool Chasing { get; set; }
        public bool Attracted { get; set; }
        public bool Leaving { get; set; }
        public bool Afraid { get; set; }


        private GameObject sightRef;

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

            // Hide detection points
            if(exclamationPoint && interrogationPoint)
            {
                interrogationPoint.SetActive(false);
                exclamationPoint.SetActive(false);
            }
            
            health = GetComponent<Health>();
            // Call EnemyManager.OnEnemyDeath when dead
            health.OnDeath += () =>
            {
                EnemyManager.OnEnemyDeath?.Invoke(this);
                foreach (var enemy in currentRoom.enemies)
                {
                    enemy.GetAfraid(transform);
                }
            };
            
            gridController.OnMoved += OnMoved;
            
            SetState(new ExplorationStateBase(this));

            stateColor = Color.yellow;
            stateColor.a = 0.1f;

            // Set light radius and range depending on the enemy stats
            if (!detectionSpotlight) detectionSpotlight = GetComponentInChildren<Light2D>();
            if (detectionSpotlight)
            {
                detectionSpotlight.pointLightOuterRadius = sightRange;
                detectionSpotlight.pointLightInnerAngle = sightAngle;
                detectionSpotlight.pointLightOuterAngle = sightAngle;
                SetLightColor(chase:false);
            }

            sightRef = transform.GetChild(0).gameObject;
        }


        protected virtual void Update() {
            currentState?.OnStateUpdate();
        }
        
        /// <summary>
        /// Do Action method, let the current state choose the action to do
        /// This method is called by the GameManager in every enemy at each turn
        /// </summary>
        public void DoAction() {
            // Delegate action to current state
            currentState?.DoAction();
        }
        
        /// <summary>
        /// Function called by the GridController after moving
        /// </summary>
        /// <param name="direction"></param>
        private void OnMoved(Direction direction)
        {
            UpdateCurrentRoom();
            
            // Kill player if next and we are chasing
            if (Chasing)
            {
                if (GridManager.Instance.GetManhattanDistance(gridController.gridPosition,
                    PlayerManager.Instance.Player.gridController.gridPosition) <= 1)
                {
                    PlayerManager.Instance.PlayerHealth.Death();
                    return;
                } 
            }
            
            if(!Chasing && !Afraid && !Attracted && !Leaving)
            {
                // Check if player is in sight
                Vector3 playerPos = PlayerManager.Instance.Player.transform.position;
                if (IsObjectInSight(playerPos))
                {
                    // Start chase
                    StartChase(PlayerManager.Instance.Player);
                    return;
                }
                
                // Check to open closes
                Curtain closest = currentRoom.GetClosestCurtain(transform.position);
                
                if(closest != null && GridManager.Instance.GetManhattanDistance(gridController.gridPosition, new Vector2Int((int)closest.transform.position.x, (int)closest.transform.position.y)) <= curtainRange 
                                   && !(currentState is EnemyInteractState) && !closest.IsOpened)    {
                    int random = Random.Range(0, 100);

                    if (random <= curtainProbability)            
                        SetState(new EnemyInteractState(this, new ExplorationStateBase(this), closest));
                }
            }


            // Rotate light in pointing direction
            if (direction == null)
                return;
            Vector2Int dir = Direction.ToVector2Int(direction.ToString());
            lastMoveDirection = dir;

            detectionSpotlight.transform.eulerAngles =
                new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);

            sightRef.transform.eulerAngles = new Vector3(Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90f,90f,-90f);
        }      


        #region Detection functions
        public bool HasLineOfSight(Vector2 targetWorldPos)
        {
            Vector2 position = transform.position;
            int manhattanDistance = gridController.gridManager.GetManhattanDistance(position, targetWorldPos);
            float distance = Vector2.Distance(position, targetWorldPos);

            float temp = distance / manhattanDistance;
            for (int i = 0; i < manhattanDistance * 2 - 1; i++)
            {
                float startF = temp * (i / (float) (manhattanDistance * 2 - 1));
                Vector2 startPos = Vector2.Lerp(position, targetWorldPos, startF);
                // float endF = temp * ((i + 1) / (float) (manhattanDistance * 2 - 1));
                // Vector2 endPos = Vector2.Lerp(position, targetWorldPos, endF);

                // Debug.Log($"start: {startF} // end: {endF}");

                bool valid = gridController.gridManager.GetTileAtPosition(startPos) != null;
                if (!valid)
                    return false;
                // Debug.DrawLine(startPos, endPos, valid ? Color.green : Color.red, 2f);
            }
            Debug.DrawLine(position, targetWorldPos, Color.green, 2f);
            return true;
        }

        public bool IsObjectInSight(Vector3 objectWorldPos)
        {
            var position = transform.position;
            
            // Check distance from object
            float distanceFromPlayer = Vector2.Distance(objectWorldPos, position);
            if (distanceFromPlayer <= sightRange)
            {
                // Check if object in sight angle
                float degAngle = Vector2.Angle(lastMoveDirection, (objectWorldPos - position).normalized);
                if (degAngle <= sightAngle * 0.5f)
                {
                    return HasLineOfSight(objectWorldPos);
                }
            }
            return false;
        }

        #endregion

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

            currentStateName = currentState.GetType().ToString();
        }

        // Add or Remove self to room enemie list when entering or exiting
        protected override void OnExitRoom(Room room) => room.enemies.Remove(this);
        protected override void OnEnterRoom(Room room) => room.enemies.Add(this);


        public void GetAfraid(Transform source)
        {
            if (!canBeAfraid)
                return;
            // Don't frigthen if already frightened
            if (Afraid)
                return;

            // Increment the count
            afraidCount++;

            // Choose what do to after being afraid depending on the bravery and the afraidCount
            EnemyBaseState afterAfraidState;
            if (afraidCount >= bravery)
            {
                Vector2Int exitGridPos =
                    gridController.gridManager.GetGridPosition(GameManager.Instance.EnemySpawnPoint.position);
                afterAfraidState = new EnemyLeaveState(this, exitGridPos);
            }
            else
            {
                afterAfraidState = new ExplorationStateBase(this);
            }
            
            // Set state to afraid
            Debug.Log("Get Afraid");
            SetState(
                new EnemyAfraidState(
                    enemy: this,
                    frighteningSource: source,
                    numberOfTurn: 3,
                    nextState: afterAfraidState
                )
            );

            if(tearsParticules)
                tearsParticules.Play();

            //ParticuleManager.Instance.OnEnemyScared();
            AudioManager.Instance.PlayEnemySound(EnemyAudioTracks.E_Fear);
        }

        public void GetAttracted(Vector2Int attractionGridPos, System.Action onArrived)
        {
            if (!canBeAttracted)
                return;
            // Don't attract if already attracted
            if (Attracted)
                return;
            
            SetState(new EnemyAttractedState(
                enemy: this, 
                aimedGridPos: attractionGridPos,
                onArrived: onArrived, 
                nextState: new ExplorationStateBase(this)
                )
            );
        }

        public void StartChase(Entity chasedEntity)
        {
            if (Chasing || Afraid || Attracted || Leaving)
                return;
            
            SetState(new EnemyChaseState(this, chasedEntity, currentState));
        }

        #region Light methods
        public void SetLightColor(bool chase)
        {
            if (detectionSpotlight)
                detectionSpotlight.color = chase ? chaseLightColor : defaultLightColor;
        }
        public void SetActiveLight(bool active)
        {
            if(detectionSpotlight)
                detectionSpotlight.gameObject.SetActive(active);
        }
        #endregion
        
        
        // IInteractable implementation
        public Transform Transform => transform;
        public bool IsInteractable => Player.transformed;
        public bool NeedToBeInRange => true;
        public bool TryInteract()
        {
            if (health.TakeDamage(1))
            {
                // If died -> call OnEnemyKilled event
                ParticuleManager.Instance.OnEnemyDeath();
                
                // Leaving count as enemy disappearing, we don't want to make the same enemy disappears twice
                EnemyManager.OnEnemyKilled?.Invoke(this);
            }
            return true;
        }
    }
}