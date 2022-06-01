using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;

public class KnightEvent : FrighteningEventBase
{
    public List<Direction> directionKnight = new List<Direction>();
    private int index = 0;

    private Vector2Int knightPos;

    public int NumberOfTurnTheKnightCanMove = 5;

    public override void Interact()
    {
        if(GameManager.dayState == GameManager.DayState.NIGHT)
        {
            Debug.Log("coucou0");
            Do();
        }
        else
        {
            return;
        }
    }


    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        EnemyManager.knights.Add(this);
        index = 0;

        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES
        Vector2Int enemyToScare = CurrentRoom.GetClosestEnemy(transform.position).GridController.gridPosition;
        knightPos = GridManager.Instance.GetGridPosition(transform.position);

        //Get the path to do
        directionKnight = GridManager.Instance.TempGetPath(knightPos, enemyToScare);

        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        // Reset the start position of the knight
        //gridController.SetPosition(knightPos);
        index = 0;
        directionKnight.Clear();

        Vector2Int actualKnightPos = GridManager.Instance.GetGridPosition(transform.position);
        directionKnight = GridManager.Instance.TempGetPath(actualKnightPos, knightPos);

        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }

    public void MoveKnight()
    {
        if(GameManager.dayState == GameManager.DayState.NIGHT)
        {
            if(index < NumberOfTurnTheKnightCanMove)
            {
                gridController.Move(directionKnight[index]);
                index++;
            }
        } else
        {
            return;
        }
    }
}
