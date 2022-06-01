using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour, IInteractable
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;

    public bool IsIn = false;

    public bool waitForVent = false;

    public bool FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

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

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            IsIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            IsIn = false;
        }
    }

    public IEnumerator VentDelay()
    {        
        yield return new WaitForSeconds(5f);
        waitForVent = false;
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

    }

    
}
