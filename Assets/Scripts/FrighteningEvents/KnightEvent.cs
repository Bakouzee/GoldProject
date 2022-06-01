using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;

public class KnightEvent : FrighteningEventBase
{
    public List<Direction> directionKnight = new List<Direction>();
    private void OnMouseDown()
    {
        Do();
    }

    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES
        Vector2Int enemyToScare = CurrentRoom.GetClosestEnemy(transform.position).GridPosition;
        Vector2Int knightPos = GridManager.Instance.GetGridPosition(transform.parent.position);
        directionKnight = GridManager.Instance.TempGetPath(knightPos, enemyToScare);

         ;

        Debug.Log("Activated");
        yield return new WaitForSeconds(50f);
        Debug.Log("done");
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }
}
