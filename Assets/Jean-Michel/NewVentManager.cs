using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject;
using AudioController;
using GridSystem;


public class NewVentManager : SingletonBase<NewVentManager>
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;

    public GameObject ventThree;

    public GameObject ventFour;

    public static bool choosingVent;

    public static Action OnVentUsed;
    

    private void Start()
    {        
        player = PlayerManager.Instance.Player;
    }

    public void FirstRoomVent() => UseVent(ventOne);
    public void SecondRoomVent() => UseVent(ventTwo);
    public void ThirdRoomVent() => UseVent(ventThree);
    public void FourthRoomVent() => UseVent(ventFour);

    private void UseVent(GameObject vent)
    {
        player.gridController.SetPosition(vent.transform.position + Vector3.left);
        gameObject.SetActive(false);
        PlayerManager playerManager = PlayerManager.Instance;
        playerManager.arrowToMovePlayer.SetActive(true);
        playerManager.Player.SetNeighborTilesWalkable();
        choosingVent = false;
        
        // ;(
        OnVentUsed?.Invoke();
    }
}
