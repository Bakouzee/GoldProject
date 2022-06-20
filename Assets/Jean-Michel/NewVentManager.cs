using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject;
using AudioController;


public class NewVentManager : SingletonBase<NewVentManager>
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;

    public GameObject ventThree;

    public GameObject ventFour;

    public static bool choosingVent;
    

    private void Start()
    {        
        player = PlayerManager.Instance.Player;
    }

    public void FirstRoomVent()
    {
        player.gridController.SetPosition(ventOne.transform.position + Vector3.left);
        gameObject.SetActive(false);
        PlayerManager.Instance.arrowToMovePlayer.SetActive(true);
        choosingVent = false;
    }

    public void SecondRoomVent()
    {
        player.gridController.SetPosition(ventTwo.transform.position + Vector3.left);
        gameObject.SetActive(false);
        PlayerManager.Instance.arrowToMovePlayer.SetActive(true);
        choosingVent = false;
    }
    public void ThirdRoomVent()
    {
        player.gridController.SetPosition(ventThree.transform.position + Vector3.left);
        gameObject.SetActive(false);
        PlayerManager.Instance.arrowToMovePlayer.SetActive(true);
        choosingVent = false;
    }

    public void FourthRoomVent()
    {
        player.gridController.SetPosition(ventFour.transform.position + Vector3.left);
        gameObject.SetActive(false);
        PlayerManager.Instance.arrowToMovePlayer.SetActive(true);
        choosingVent = false;
    }
}
