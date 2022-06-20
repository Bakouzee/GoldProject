using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AudioController;
using GridSystem;

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

    //public Animator possibleToVent;

    private void Start()
    {
        // Snap position to grid
        Vector2 newWorldPos = transform.position;
        newWorldPos = new Vector2(Mathf.Floor(newWorldPos.x), Mathf.Floor(newWorldPos.y)) + Vector2.one * 0.5f;
        transform.position = newWorldPos; 

        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;
        player = PlayerManager.Instance.Player;

        // Set vent system if unassigned in the inspector
        if (!ventSysteme)
        {
            var newVentManager = FindObjectOfType<NewVentManager>();
            if (newVentManager) ventSysteme = newVentManager.gameObject;
        }
    }


    public bool TryInteract()
    {
        if (!IsInteractable || waitForVent)
            return false;

        AudioManager.Instance.PlayVentSound(VentAudioTracks.V_Use);

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
        if (vent > 0)
        {
            ventOne.GetComponent<BoxCollider2D>().enabled = false;

            ventOne.GetComponent<SpriteRenderer>().sprite = spriteClosed;

            ventOne.GetComponent<SpriteRenderer>().color = Color.red;

            ventOne.GetComponent<Animator>().enabled = false;



            ventTwo.GetComponent<BoxCollider2D>().enabled = false;

            ventTwo.GetComponent<SpriteRenderer>().sprite = spriteClosed;

            ventTwo.GetComponent<SpriteRenderer>().color = Color.red;

            ventTwo.GetComponent<Animator>().enabled = false;




            ventThree.GetComponent<BoxCollider2D>().enabled = false;

            ventThree.GetComponent<SpriteRenderer>().sprite = spriteClosed;

            ventThree.GetComponent<SpriteRenderer>().color = Color.red;

            ventThree.GetComponent<Animator>().enabled = false;




            ventFour.GetComponent<BoxCollider2D>().enabled = false;

            ventFour.GetComponent<SpriteRenderer>().sprite = spriteClosed;

            ventFour.GetComponent<SpriteRenderer>().color = Color.red;

            ventFour.GetComponent<Animator>().enabled = false;
        }
        else
        {
            waitForVent = false;
            FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

            ventOne.GetComponent<BoxCollider2D>().enabled = true;

            ventOne.GetComponent<SpriteRenderer>().sprite = spriteOpen;

            ventOne.GetComponent<SpriteRenderer>().color = Color.white;

            ventOne.GetComponent<Animator>().enabled = true;



            ventTwo.GetComponent<BoxCollider2D>().enabled = true;

            ventTwo.GetComponent<SpriteRenderer>().sprite = spriteOpen;

            ventTwo.GetComponent<SpriteRenderer>().color = Color.white;

            ventTwo.GetComponent<Animator>().enabled = true;




            ventThree.GetComponent<BoxCollider2D>().enabled = true;

            ventThree.GetComponent<SpriteRenderer>().sprite = spriteOpen;

            ventThree.GetComponent<SpriteRenderer>().color = Color.white;

            ventThree.GetComponent<Animator>().enabled = true;




            ventFour.GetComponent<BoxCollider2D>().enabled = true;

            ventFour.GetComponent<SpriteRenderer>().sprite = spriteOpen;

            ventFour.GetComponent<SpriteRenderer>().color = Color.white;

            ventFour.GetComponent<Animator>().enabled = true;


        }
    }
}