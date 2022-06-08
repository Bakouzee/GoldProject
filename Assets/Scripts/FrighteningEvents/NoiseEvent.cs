using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using AudioController;
using Enemies;

public class NoiseEvent : FrighteningEventBase
{
    private Animator anim;
    private SpriteRenderer srParent;
    private SpriteRenderer srMap;

    public string animationTrigger;

    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        srParent = transform.parent.GetComponent<SpriteRenderer>();
        srParent.enabled = false;

        srMap = GetComponent<SpriteRenderer>();
    }

    public override bool TryInteract()
    {
        if (!base.TryInteract())
            return false;
        
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him
        Debug.Log("NoiseTrap");
        Do();

        return true;
    }

    // Check if enemy is in range to activate the animation
    protected override IEnumerator DoActionCoroutine()
    {
        if (CurrentRoom.enemies.Count == 0)
        {
            Debug.Log("No enemy in sight -> the trap didn't work !");
            srMap.color = Color.red;
            srParent.enabled = true;
            anim.SetTrigger(animationTrigger);
            yield break;
        }

        foreach(EnemyBase enemy in CurrentRoom.enemies)
        {
            int directionBetweenTrapAndEnemy = GridManager.Instance.GetManhattanDistance(transform.position, enemy.transform.position);

            Debug.Log(directionBetweenTrapAndEnemy);
            
            // If enemies are directly next to the trap -> they will be scared !
            if(directionBetweenTrapAndEnemy < distanceToBeScared)
            {
                enemy.GetAfraid(transform);
            }
        }

        //Launch animation
        srMap.color = Color.green;
        srParent.enabled = true;
        anim.SetBool(animationTrigger, true);

        yield return new WaitForSeconds(1f);
        Debug.Log("done");
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        Color32 readyColor = new Color32(166, 79, 0, 255);
        srMap.color = readyColor;
        // Reset animation
        anim.SetBool(animationTrigger, false);
        yield return new WaitForSeconds(1f);
        srParent.enabled = false;
        Debug.Log("undone");
    }
}
