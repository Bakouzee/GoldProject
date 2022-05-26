using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour
{
    public GameObject player;
    public GameObject ventOne;

    public GameObject ventTwo;

    public bool IsIn = false;



    private void Update()
    {
        if(IsIn == true && Input.GetKeyDown(KeyCode.P))
        {
            player.transform.position = ventTwo.transform.position;
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
}
