using System;
using System.Collections;
using System.Collections.Generic;
using GoldProject;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Player Player { get; private set; } 
    public PlayerHealth PlayerHealth { get; private set; } 

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
    }

    private void Start()
    {
        if(miniMap) miniMap.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mapSeen = !mapSeen;
            ShowMap(mapSeen);
        }
    }

    private void ShowMap(bool canSeeMap)
    {
        if (miniMap != null)
        {
            if (canSeeMap)
            {
                miniMap.SetActive(true);
                mainCam.SetActive(false);
            } else
            {
                mainCam.SetActive(true);
                miniMap.SetActive(false);
            }
        }
    }
}
