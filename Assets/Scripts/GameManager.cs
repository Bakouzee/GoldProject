using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enemies;
using GoldProject;
using GoldProject.Rooms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    public enum DayState
    {
        DAY,
        NIGHT
    };

    public static DayState dayState = DayState.DAY;
    public static EventSystem eventSystem;

    [Header("Turns")] [SerializeField] private int actionPerPhase;
    private int actionCount;
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
    }

    private void Update()
    {
        if (turnCooldown.HasCooldown())
            LaunchTurn();

        if(dayState == DayState.DAY)
        {
            undoButton.SetActive(false);
        }
        else
        {
            undoButton.SetActive(true);
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

    public System.Action<int> OnLaunchedTurn;
    public void LaunchTurn()
    {
        // Enemies make their turn
        foreach (var enemy in EnemyManager.enemies)
        {
            enemy.DoAction();
        }

        // Knight too
        foreach(var knight in EnemyManager.knights)
        {
            knight.MoveKnight();
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

        // Cooldown of turn
        turnCooldown.SetCooldown();
        
        OnLaunchedTurn?.Invoke(actionCount);
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
            Instantiate(prefab, enemySpawnPoint.position, Quaternion.identity);
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
        [Range(1, 50)]
        public int spawnOrder;
    }

    [System.Serializable]
    public struct EnemyCount
    {
        public Enemies.EnemyType enemyType;
        public int count;
    }
}