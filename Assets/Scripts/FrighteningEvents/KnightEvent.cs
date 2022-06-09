using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using Enemies;
using AudioController;

public class KnightEvent : FrighteningEventBase
{
    private EnemyBase enemyToScare;
    private SpriteRenderer[] srMap;
    private Animator animKnight;

    public List<Direction> directionKnight = new List<Direction>();
    private Vector2Int knightPos;

    public int NumberOfTurnTheKnightCanMove = 5;
    private int index = 0;

    private bool isReseting;

    private void Start()
    {
        // srMap[1] -> child srMap[0] -> this
        // need to be in range quand la map est désactivé
        srMap = GetComponentsInChildren<SpriteRenderer>();

        gridController = new GridController(transform);

        knightPos = GridManager.Instance.GetGridPosition(transform.position);

        animKnight = GetComponent<Animator>();

        isReseting = false;

        PlayerManager.Instance.onShowMap += () =>
        {
            needToBeInRange = PlayerManager.mapSeen;
            Debug.Log(needToBeInRange);
        };
    }

    public override bool TryInteract()
    {
        if (!base.TryInteract())
            return false;
        
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him

        if (!isReseting)
        {
            Debug.Log("KnightTrap");
            Do();
            return true;
        } else
        {
            Debug.Log("Trap didn't work");
            return false;
        }

    }


    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        if (CurrentRoom.enemies.Count == 0)
        {
            Debug.Log("No enemy in sight -> the trap didn't work !");
            srMap[1].color = Color.red;
            isReseting = false;
            yield break;
        }

        EnemyManager.knights.Add(this);

        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES

        // The trap worked
        srMap[1].color = Color.green;

        enemyToScare = CurrentRoom.GetClosestEnemy(transform.position);
        Debug.Log("Enemy found : " + enemyToScare.name);

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
        // gridController.SetPosition(knightPos);

        index = 0;
        directionKnight.Clear();

        Vector2Int actualKnightPos = GridManager.Instance.GetGridPosition(transform.position);
        directionKnight = GridManager.Instance.GetPath(actualKnightPos, knightPos);

        isReseting = CurrentRoom.enemies.Count > 0;

        if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
        {
            isReseting = false;
            Debug.Log("It worked");
        }

            yield return new WaitForSeconds(1f);

        Color32 readyColor = new Color32(166, 79, 0, 255);
        srMap[1].color = readyColor;

        Debug.Log("undone");
    }

    public void MoveKnight()
    {
        //Debug.Log("index : " + index);
        if (index < NumberOfTurnTheKnightCanMove && !isReseting)
        {
            // Check if the knight is at distance to scare the enemy
            List<Direction> refreshedDistance = GridManager.Instance.GetPath(knightPos, enemyToScare.GridController.gridPosition);
            if (refreshedDistance.Count < distanceToBeScared)
            {
                enemyToScare.GetAfraid(transform);
            }

            // Move the knight until he can't
            if (index < directionKnight.Count)
            {
                if (gridController != null)
                {
                    gridController.Move(directionKnight[index], animKnight);
                    AudioManager.Instance.PlayScarySound(ScaryAudioTracks.Sc_StatueWalk);
                    index++;
                }
            }

            if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
            {
                if (index == directionKnight.Count)
                {
                    EnemyManager.knights.Remove(this);
                    index = 0;
                }
            }
        }

        if (isReseting && GameManager.dayState == GameManager.DayState.NIGHT)
        {
            if (index < directionKnight.Count)
            {
                if (gridController != null)
                {
                    gridController.Move(directionKnight[index], animKnight);
                    index++;
                }
            }

            if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
            {
                animKnight.SetTrigger("Down");
                if (index == directionKnight.Count)
                {
                    EnemyManager.knights.Remove(this);
                    isReseting = false;
                    index = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        EnemyManager.knights.Remove(this);
    }
}