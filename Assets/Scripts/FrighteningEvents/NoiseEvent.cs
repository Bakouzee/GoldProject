using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using AudioController;
public class NoiseEvent : FrighteningEventBase
{
    public Animator anim;
    
    public string animationTrigger;

    private void OnMouseDown()
    {
        if (GameManager.dayState == GameManager.DayState.DAY)
        {
            // normally have to activate the trap AND WHEN an enemy is at his range or in the room
            // the armor will move to him
            Debug.Log("NoiseTrap");
            Do();
        }
        else
        {
            return;
        }
    }

    // animation event
    public void MakeScarySound(ScaryAudioTracks audioToPlay)
    {
        AudioManager.Instance.PlayScarySound(audioToPlay);
    }

    // Check if enemy is in range to activate the animation
    protected override IEnumerator DoActionCoroutine()
    {
        //Launch animation
        anim.SetTrigger(animationTrigger);

        yield return new WaitForSeconds(1f);
        Debug.Log("done");
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        anim.SetTrigger("ResetAnim");
        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }
}
