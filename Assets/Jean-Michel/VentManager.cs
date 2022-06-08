using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VentManager : MonoBehaviour, IInteractable
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;
    public GameObject ventThree;
    public GameObject ventFour;

    public GameObject ventSysteme;  




    public bool waitForVent = false;

    public bool FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

    public Transform Transform => transform;
    public bool IsInteractable => true;
    public bool NeedToBeInRange => true;

    public Sprite spriteClosed;

    public Sprite spriteOpen;

    private void Start()
    {
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;
        player = PlayerManager.Instance.Player;
    }

    

    public bool TryInteract()
    {
        if (!IsInteractable || waitForVent)
            return false;
        
        ventSysteme.SetActive(true);
        NewVentManager.choosingVent = true;
        PlayerManager.Instance.arrowToMovePlayer.SetActive(false);
        GameManager.Instance.actionCountForVent = 10;

        waitForVent = true;
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = false;
        return true;
    }
    
    public void LaunchTurnVent(int vent)
    {
        
        if(vent > 0)
        {
            ventOne.GetComponent<BoxCollider2D>().enabled = false;
            
            ventOne.GetComponent<SpriteRenderer>().sprite = spriteClosed;
            


            ventTwo.GetComponent<BoxCollider2D>().enabled = false;
            
            ventTwo.GetComponent<SpriteRenderer>().sprite = spriteClosed;


            ventThree.GetComponent<BoxCollider2D>().enabled = false;
            
            ventThree.GetComponent<SpriteRenderer>().sprite = spriteClosed;


            ventFour.GetComponent<BoxCollider2D>().enabled = false;
           
            ventFour.GetComponent<SpriteRenderer>().sprite = spriteClosed;


        }
        else
        {
            waitForVent = false;
            FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;
            ventOne.GetComponent<BoxCollider2D>().enabled = true;
           
            ventOne.GetComponent<SpriteRenderer>().sprite = spriteOpen;

            ventTwo.GetComponent<BoxCollider2D>().enabled = true;
            
            ventTwo.GetComponent<SpriteRenderer>().sprite = spriteOpen;


            ventThree.GetComponent<BoxCollider2D>().enabled = true;
            
            ventThree.GetComponent<SpriteRenderer>().sprite = spriteOpen;


            ventFour.GetComponent<BoxCollider2D>().enabled = true;
           
            ventFour.GetComponent<SpriteRenderer>().sprite = spriteOpen;

        }
    }

 

}
