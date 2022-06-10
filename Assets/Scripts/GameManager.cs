using System.Collections.Generic;
using Enemies;
using GoldProject;
using GoldProject.Rooms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AudioController;
using PlayStoreScripts;

public class GameManager : SingletonBase<GameManager>
{
    public VentManager[] vm;

    public enum DayState
    {
        DAY,
        NIGHT
    };


    public int actionCountForVent = 0;


    public static DayState dayState = DayState.DAY;
    public static EventSystem eventSystem;

    [Header("Turns")] public int actionPerPhase;
    public int actionCount;
    private int currentDay;
    public System.Action<int> OnDayChanged;

    public int CurrentDay
    {
        get => currentDay;
        private set
        {
            currentDay = Mathf.Clamp(value, 1, int.MaxValue);
            OnDayChanged?.Invoke(currentDay);
        }
    }

    [SerializeField] private Vector2 dayNightTurnCooldown;
    private Cooldown turnCooldown;
    public GameObject undoButton;

    [Header("Waves and enemy spawns")]
    // Waves
    [SerializeField]
    private TypeAndPrefabs<Enemies.EnemyChiefType> enemiesChiefDef;

    [SerializeField] private TypeAndPrefabs<Enemies.EnemyType> enemiesDef;
    [SerializeField] private Wave[] waves;

    private Wave CurrentWave
    {
        get
        {
            int index = CurrentDay - 1;
            if (index >= waves.Length)
                return waves[^1];
            return waves[index];
        }
    }

    // Enemy Spawn
    [SerializeField] private Transform enemySpawnPoint;
    private bool spawningEnemies;
    private bool chiefSpawned;
    private Enemies.EnemyType[] enemiesToSpawn;
    private int enemySpawned = 0;

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        // Set turn cooldown
        turnCooldown = new Cooldown(dayNightTurnCooldown.x);

        // Initialize dictionnaries <EnemyType, EnemyBase>
        enemiesDef.Init();
        enemiesChiefDef.Init();

        // Init days
        currentDay = 0;
        StartDay();

        OnDayStart += () =>
        {
            AudioManager.Instance.PlayEnemySound(EnemyAudioTracks.E_Entrance);
            AudioManager.Instance.PlayAmbianceSound(AmbianceAudioTracks.Cocorico);
        };

        OnNightStart += () => AudioManager.Instance.PlayAmbianceSound(AmbianceAudioTracks.Thunder);
        
        Achievements.Unlock(Achievements.BOO);
    }

    private void Update()
    {
        if (turnCooldown.HasCooldown())
            LaunchTurn();


        if (dayState == DayState.DAY)
        {
            if (undoButton != null) undoButton.SetActive(false);
        }
        else
        {
            if (undoButton != null) undoButton.SetActive(true);
        }
    }

    #region Start phases

    public System.Action OnDayStart;
    public System.Action OnNightStart;

    public void StartDay()
    {
        Debug.Log("Day");
        StartPhaseBase();
        CurrentDay++;
        dayState = DayState.DAY;

        StartSpawningWave();

        // Set turn cooldown
        turnCooldown.cooldownDuration = dayNightTurnCooldown.x;

        Curtain.SetDay(true);

        OnDayStart?.Invoke();
    }

    public void StartNight()
    {
        Debug.Log("Night");
        StartPhaseBase();
        dayState = DayState.NIGHT;

        // Set turn cooldown
        turnCooldown.cooldownDuration = dayNightTurnCooldown.y;

        Curtain.SetDay(false);

        OnNightStart?.Invoke();
    }

    private void StartPhaseBase()
    {
        actionCount = 0;
    }

    #endregion

    public System.Action<int, int> OnLaunchedTurn;

    public void LaunchTurn()
    {
        try
        {
            // Enemies make their turn
            foreach (var enemy in EnemyManager.enemies)
            {
                try
                {
                    enemy.DoAction();
                }
                catch
                {
                    Debug.LogError($"{enemy} DoAction method crashed", enemy);
                }
            }

            // Knight too
            foreach (var knight in EnemyManager.knights)
            {
                knight.MoveKnight();
            }

            for (int i = 0; i < vm.Length; i++)
            {
                vm[i].LaunchTurnVent(actionCountForVent);
            }

            if (actionCountForVent > 0)
            {
                actionCountForVent--;
            }

            // Spawn enemies
            SpawnCurrentEnemy();

            // Count 
            actionCount++;
            if (actionCount >= actionPerPhase)
            {
                if (dayState == DayState.DAY)
                    StartNight();
                else if (dayState == DayState.NIGHT)
                    StartDay();
            }
        }
        catch
        {
            Debug.LogError("Something went wrong during LaunchTurn", this);
            // ignored
        }

        // Cooldown of turn
        turnCooldown.SetCooldown();

        OnLaunchedTurn?.Invoke(actionCount, actionPerPhase);
    }

    private void StartSpawningWave()
    {
        spawningEnemies = true;
        chiefSpawned = false;
        enemiesToSpawn = CurrentWave.ToArray();
        enemySpawned = 0;
    }

    private void SpawnCurrentEnemy()
    {
        if (!spawningEnemies)
            return;

        if (!chiefSpawned)
        {
            // If it is time to spawn chief --> spawn it
            if (enemySpawned == CurrentWave.chief.spawnOrder - 1)
            {
                // Spawn chief and return
                EnemyBase chiefPrefab = enemiesChiefDef.dict[CurrentWave.chief.chiefType];
                Instantiate(chiefPrefab, enemySpawnPoint.position, Quaternion.identity);
                chiefSpawned = true;
                return;
            }
        }

        // Else if it isn't chief --> Spawn enemy
        if (enemySpawned < enemiesToSpawn.Length)
        {
            EnemyBase prefab = enemiesDef.dict[enemiesToSpawn[enemySpawned]];
            EnemyBase enemyIns = Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity);
            enemyIns.name = enemySpawned + "";
        }

        enemySpawned++;

        if (enemySpawned >= enemiesToSpawn.Length && chiefSpawned)
            StopSpawningWave();
    }


    private void StopSpawningWave()
    {
        spawningEnemies = false;
    }

    #region UI Methods*

    public void ActivateTrap(Button trapButton)
    {
        // Have to reset if the player reactivates the trap
        ColorBlock colorTrapActivated = new ColorBlock();
        colorTrapActivated.disabledColor = Color.green;
        colorTrapActivated.colorMultiplier = 1;
        trapButton.interactable = false;
        trapButton.colors = colorTrapActivated;
        Debug.Log("Trap Activated");
    }

    #endregion

    [System.Serializable]
    public struct Wave
    {
        public ChiefOrder chief;
        public EnemyCount[] enemies;

        public Enemies.EnemyType[] ToArray()
        {
            List<Enemies.EnemyType> enemyTypes = new List<EnemyType>();
            // Add enemies to the list
            foreach (EnemyCount enemyCount in enemies)
            {
                for (int i = 0; i < enemyCount.count; i++)
                    enemyTypes.Add(enemyCount.enemyType);
            }

            return enemyTypes.ToArray();
        }
    }

    [System.Serializable]
    public struct ChiefOrder
    {
        public Enemies.EnemyChiefType chiefType;
        [Range(1, 50)] public int spawnOrder;
    }

    [System.Serializable]
    public struct EnemyCount
    {
        public Enemies.EnemyType enemyType;
        public int count;
    }
}