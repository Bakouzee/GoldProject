using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour, IInteractable
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;


  

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

   

    public void Interact()
    {
        if (!waitForVent)
        {
            //player.transform.position = ventTwo.transform.position;
            player.gridController.SetPosition(ventTwo.transform.position);
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
        
        yield return new WaitForSeconds(5f);
        waitForVent = false;
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

        ventOne.GetComponent<BoxCollider2D>().enabled = true;
        ventOne.GetComponent<SpriteRenderer>().color = Color.white;
        ventTwo.GetComponent<BoxCollider2D>().enabled = true;
        ventTwo.GetComponent<SpriteRenderer>().color = Color.white;





    }


}
