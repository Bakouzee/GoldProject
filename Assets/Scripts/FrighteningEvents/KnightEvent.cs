using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using Enemies;

public class KnightEvent : FrighteningEventBase
{
    public List<Direction> directionKnight = new List<Direction>();
    private int index = 0;
    private EnemyBase enemyToScare;
    private Vector2Int knightPos;

    public int NumberOfTurnTheKnightCanMove = 5;

    public override void Interact()
    {
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him
        Debug.Log("KnightTrap");
        Do();
    }


    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        if (CurrentRoom.enemies.Count == 0)
        {
            Debug.Log("No enemy in sight -> the trap didn't work !");
            yield break;
        }


        Enemies.EnemyManager.knights.Add(this);
        index = 0;

        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES


        enemyToScare = CurrentRoom.GetClosestEnemy(transform.position);
        knightPos = GridManager.Instance.GetGridPosition(transform.position);

        //Get the path to do
        directionKnight = GridManager.Instance.GetPath(knightPos, enemyToScare.GridController.gridPosition);

        // If the enemy is directly next to the trap -> he will be scared !
        if(directionKnight.Count < distanceToBeScared)
        {
            enemyToScare.GetAfraid(transform);
        }
        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        // Reset the start position of the knight
        //gridController.SetPosition(knightPos);
        index = 0;
        directionKnight.Clear();

        Vector2Int actualKnightPos = GridManager.Instance.GetGridPosition(transform.position);
        directionKnight = GridManager.Instance.GetPath(actualKnightPos, knightPos);

        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }

    public void MoveKnight()
    {
        if(GameManager.dayState == GameManager.DayState.NIGHT)
        {
            if(index < NumberOfTurnTheKnightCanMove)
            {
                // Check if the knight is at distance to scare the enemy
                List<Direction> refreshedDistance = GridManager.Instance.GetPath(knightPos, enemyToScare.GridController.gridPosition);
                if(refreshedDistance.Count < distanceToBeScared)
                {
                    enemyToScare.GetAfraid(transform);
                }

                // Move the knight until he can't
                if(index < directionKnight.Count)
                {
                    gridController.Move(directionKnight[index]);
                }
                index++;
            }
        } else
        {
            return;
        }
    }
}