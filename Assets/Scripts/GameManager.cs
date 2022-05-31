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
    private System.Action<int> OnDayChanged;

    private int CurrentDay
    {
        get => currentDay;
        set
        {
            currentDay = Mathf.Clamp(value, 1, int.MaxValue);
            OnDayChanged?.Invoke(currentDay);
        }
    }

    [SerializeField] private Cooldown turnCooldown;
    public Camera minimapCam;

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

    [Header("UI")] private DayCounter dayCounter;

    private void Start()
    {
        InitUI();
        eventSystem = FindObjectOfType<EventSystem>();

        // Set turn cooldown
        turnCooldown.SetCooldown();

        // Initialize dictionnaries <EnemyType, EnemyBase>
        enemiesDef.Init();
        enemiesChiefDef.Init();

        // Init days
        CurrentDay = 1;
    }

    private void Update()
    {
        if (turnCooldown.HasCooldown())
            LaunchTurn();
    }

    #region Start phases
    public void StartDay()
    {
        StartPhaseBase();
        CurrentDay++;
        dayState = DayState.DAY;
        // EnemyManager.enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // can be changed by "FindGameObjectsOfType<>"
        StartSpawningWave();
        Debug.Log("Day");

        Curtain.SetDay(true);
        PlayerManager.Instance.Player.UnTransform();
    }

    public void StartNight()
    {
        StartPhaseBase();
        dayState = DayState.NIGHT;
        // EnemyManager.enemies.Clear(); // Reset all enemies in the list
        Debug.Log("Night");
        
        Curtain.SetDay(false);
        PlayerManager.Instance.Player.Transform();
    }

    private void StartPhaseBase()
    {
        actionCount = 0;
    }

    #endregion

    public void LaunchTurn()
    {
        // Enemies make their turn
        foreach (var enemy in EnemyManager.enemies)
        {
            enemy.DoAction();
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
            if (enemySpawned == CurrentWave.chief.spawnOrder)
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

    #region UI Methods

    private void InitUI()
    {
        dayCounter = FindObjectOfType<DayCounter>();
        OnDayChanged += i => dayCounter.SetDay(i);
    }

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
        public int spawnOrder;
    }

    [System.Serializable]
    public struct EnemyCount
    {
        public Enemies.EnemyType enemyType;
        public int count;
    }
}