using System;
using System.Collections;
using System.Collections.Generic;
using GoldProject;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Enemies;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Player Player { get; private set; } 
    public PlayerHealth PlayerHealth { get; private set; } 
    public PlayerBonuses Bonuses { get; private set; }

    public TextMeshProUGUI textEnemyTrap;
    public GameObject arrowToMovePlayer;
    public GameObject miniMap;
    public GameObject mainCam;
    public static bool mapSeen = false;

    protected override void Awake()
    {
        base.Awake();
        Player = GetComponent<Player>();
        Player.PlayerManager = this;
        
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerHealth.PlayerManager = this;

        Bonuses = GetComponent<PlayerBonuses>();
        Bonuses.PlayerManager = this;
    }

    private void Start()
    {
        if(miniMap) miniMap.SetActive(false);
    }

    public void ShowMap()
    {
        if (miniMap != null)
        {
            if (!mapSeen)
            {
                foreach(EnemyBase enemy in Player.CurrentRoom.enemies)
                {
                    enemy.gameObject.layer = 3;
                }
                miniMap.SetActive(true);
                arrowToMovePlayer.SetActive(false);
                textEnemyTrap.text = "Show Enemies";
                mainCam.SetActive(false);
            } else
            {
                foreach (EnemyBase enemy in Player.CurrentRoom.enemies)
                {
                    enemy.gameObject.layer = 0;
                }
                mainCam.SetActive(true);
                arrowToMovePlayer.SetActive(true);
                textEnemyTrap.text = "Show Traps";
                miniMap.SetActive(false);
            }
        }
        mapSeen = !mapSeen;
    }
}
