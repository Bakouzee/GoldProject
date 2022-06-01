using GoldProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour, IInteractable
{
    public Player player;
    public GameObject ventOne;

    public GameObject ventTwo;

    public GameObject red1;

    public GameObject red2;
  

    public bool waitForVent = false;

    public bool FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;

    public bool IsInteractable => true;

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
        
        ventTwo.SetActive(false);
        red1.SetActive(true);
        red2.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        waitForVent = false;
        FreddyWithTwoRingOnHisHandBecauseOfCeWeekendDeFolieOuIlAGraveKiffé = true;
        ventTwo.SetActive(true);
        red1.SetActive(false);
        red2.SetActive(false);

        


    }


}
