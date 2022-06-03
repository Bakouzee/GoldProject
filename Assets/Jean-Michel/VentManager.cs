using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour, IInteractable
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;
    public GameObject ventThree;
    public GameObject ventFour;

    public GameObject ventSysteme;

    private int actionCount = 20;




    public bool waitForVent = false;

    public bool FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

    public Transform Transform => transform;
    public bool IsInteractable => true;
    public bool NeedToBeInRange => true;

    private void Start()
    {
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;
        player = PlayerManager.Instance.Player;
    }

    //public System.Action<int> OnLaunchedTurn;

    public void Interact()
    {
        if (!waitForVent)
        {
            
            ventSysteme.SetActive(true);
            NewVentManager.choosingVent = true;
            PlayerManager.Instance.arrowToMovePlayer.SetActive(false);  
            waitForVent = true;
            FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = false;
        }
        if (waitForVent)
        {            
            StartCoroutine(VentDelay());
        }
    }    

    public IEnumerator VentDelay()
    {

        ventOne.GetComponent<BoxCollider2D>().enabled = false;
        ventOne.GetComponent<SpriteRenderer>().color = Color.red;

        ventTwo.GetComponent<BoxCollider2D>().enabled = false;
        ventTwo.GetComponent<SpriteRenderer>().color = Color.red;

        ventThree.GetComponent<BoxCollider2D>().enabled = false;
        ventThree.GetComponent<SpriteRenderer>().color = Color.red;

        ventFour.GetComponent<BoxCollider2D>().enabled = false;
        ventFour.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(10f);
        //OnLaunchedTurn?.Invoke(actionCount);
        waitForVent = false;
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

        ventOne.GetComponent<BoxCollider2D>().enabled = true;
        ventOne.GetComponent<SpriteRenderer>().color = Color.white;

        ventTwo.GetComponent<BoxCollider2D>().enabled = true;
        ventTwo.GetComponent<SpriteRenderer>().color = Color.white;

        ventThree.GetComponent<BoxCollider2D>().enabled = true;
        ventThree.GetComponent<SpriteRenderer>().color = Color.white;

        ventFour.GetComponent<BoxCollider2D>().enabled = true;
        ventFour.GetComponent<SpriteRenderer>().color = Color.white;





    }


}
