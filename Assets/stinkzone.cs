using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stinkzone : MonoBehaviour
{
    public int damageOnCollision = 5;    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeStinkDamage(damageOnCollision);
        }
    }

}
