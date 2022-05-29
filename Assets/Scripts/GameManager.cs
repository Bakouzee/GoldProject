using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using GoldProject;
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

    public Camera minimapCam;
    [SerializeField] private Cooldown turnCooldown;

    [Space(10), SerializeField] private TypeAndPrefab[] enemyTypesAndPrefabs;
    private Dictionary<Enemies.EnemyType, Enemies.EnemyBase> enemyTypesAndPrefabsDict = new Dictionary<EnemyType, EnemyBase>();
    [Space(10), SerializeField] private Wave[] waves;
    // Days starts at 0
    int currentDay;
    

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        
        // Set turn cooldown
        turnCooldown.SetCooldown();

        // Initialize dictionnary of enemy types and prefabs
        foreach (var typeAndPrefab in enemyTypesAndPrefabs)
        {
            enemyTypesAndPrefabsDict.Add(typeAndPrefab.type, typeAndPrefab.prefab);
        }
    }

    private void Update()
    {
        if (turnCooldown.HasCooldown())
            MoveAllEnemies();
    }

    public void StartDay()
    {
        dayState = DayState.DAY;
        // EnemyManager.enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // can be changed by "FindGameObjectsOfType<>"
    }
    
    public void StartNight()
    {
        dayState = DayState.NIGHT;
        EnemyManager.enemies.Clear(); // Reset all enemies in the list
    }

    public void MoveAllEnemies()
    {
        // Debug.Log("Enemy turn");
        foreach (var enemy in EnemyManager.enemies)
        {
            enemy.DoAction();
        }
        turnCooldown.SetCooldown();
    }

    #region UI Methods

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
        public EnemyCount[] enemyCounts;
    }

    [System.Serializable]
    public struct EnemyCount
    {
        public Enemies.EnemyType enemyType;
        public int count;
    }
}
