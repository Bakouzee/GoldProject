using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using AudioController;
using Enemies;

public class NoiseEvent : FrighteningEventBase
{
    public Animator anim;
    
    public string animationTrigger;

    public override void Interact()
    {
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him
        Debug.Log("NoiseTrap");
        Do();

    }

    // animation event
    public void MakeScarySound(ScaryAudioTracks audioToPlay)
    {
        AudioManager.Instance.PlayScarySound(audioToPlay);
    }

    // Check if enemy is in range to activate the animation
    protected override IEnumerator DoActionCoroutine()
    {
        Vector2Int thisPos = GridManager.Instance.GetGridPosition(transform.position);

        foreach(EnemyBase enemy in CurrentRoom.enemies)
        {
            List<Direction> directionBetweenTrapAndEnemy = GridManager.Instance.GetPath(thisPos, enemy.GridController.gridPosition);
            Debug.Log(directionBetweenTrapAndEnemy.Count);
            
            // If enemies are directly next to the trap -> they will be scared !
            if(directionBetweenTrapAndEnemy.Count < distanceToBeScared)
            {
                enemy.GetAfraid(transform);
            }
        }

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
