using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;

public class NoiseEvent : FrighteningEventBase
{
    private Animator anim;
    
    public string animationTrigger;

    private void Start()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnMouseDown()
    {
        Do();
    }

    // animation event
    public void MakeScarySound()
    {

    }

    // Check if enemy is in range to activate the animation
    protected override IEnumerator DoActionCoroutine()
    {
        Vector2Int enemyToScare = CurrentRoom.GetClosestEnemy(transform.position).GridController.gridPosition;
        Vector2Int trapPos = GridManager.Instance.GetGridPosition(transform.parent.position);

        if(Vector2Int.Distance(enemyToScare, trapPos) <= 3)
        {
            //Launch animation
            anim.SetTrigger(animationTrigger);
        }

        yield return new WaitForSeconds(1f);
        Debug.Log("done");
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }
}
