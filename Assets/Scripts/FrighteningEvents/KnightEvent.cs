using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;

public class KnightEvent : FrighteningEventBase
{
    public List<Direction> directionKnight = new List<Direction>();
    private int index = 0;

    public int NumberOfTurnTheKnightCanMove = 5;

    public override void Interact()
    {
        /*if (transform.parent != null && transform.parent.GetComponent<KnightEvent>())
        {
            transform.parent.GetComponent<KnightEvent>().Do();
        }*/
        Debug.Log("coucou0");
        Do();
    }


    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        EnemyManager.knights.Add(this);
        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES
        Vector2Int enemyToScare = CurrentRoom.GetClosestEnemy(transform.position).GridController.gridPosition;
        Vector2Int knightPos = GridManager.Instance.GetGridPosition(transform.position);

        //Get the path to do
        directionKnight = GridManager.Instance.TempGetPath(knightPos, enemyToScare);
        foreach (Direction direction in directionKnight)
        {
            Debug.Log(direction.Value);
        }

        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }

    public void MoveKnight()
    {
        if(index < NumberOfTurnTheKnightCanMove)
        {
            gridController.Move(directionKnight[index]);
            index++;
        }
    }
}
